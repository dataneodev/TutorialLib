using Ardalis.Specification;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class TutorialRespositoryAsync : EfRepository<Tutorial>, ITutorialRespositoryAsync, IDisposable
    {
        private readonly float Megabytes = 1048576f;
        public TutorialRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public async Task<IReadOnlyList<TutorialHeaderDto>> GetAllTutorialHeadersDtoAsync(
                        ISpecification<Tutorial> spec,
                        CancellationToken cancellationToken = default)
        {
            var watchProgressFractor = Episode.WatchPercentage / 100f;
            var specificationResult = ApplySpecification(spec);
            var result = await specificationResult
                .Select(s =>
                    new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        DateAdd = s.AddDate,
                        Rating = s.Rating,
                        LastPlayedDate = s.Folders.Max(m => m.Episodes.Max(se => se.LastPlayedDate)),
                        TotalTime = s.Folders.Sum(w => w.Episodes.Sum(se => se.File.PlayTimeSecond)),
                        TimePlayed = s.Folders.Sum(w => w.Episodes.Sum(se => se.PlayedTimeSecond)),
                        PlayedEpisodes = (short)s.Folders.Sum(w =>
                                            w.Episodes.Count(w => w.PlayedTimeSecond > w.File.PlayTimeSecond * watchProgressFractor)),
                        TotalEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count()),
                        TotalSizeMB = (float)s.Folders.Sum(w => w.Episodes.Sum(se => se.File.FileSize / Megabytes)),
                        Categories = s.Categories,
                    })
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.Select(s =>
                                new TutorialHeaderDto(
                                    Id: s.Id,
                                    Name: s.Name,
                                    TotalTime: TimeSpan.FromSeconds(s.TotalTime),
                                    TimePlayed: TimeSpan.FromSeconds(s.TimePlayed),
                                    PlayedEpisodes: s.PlayedEpisodes,
                                    TotalEpisodes: s.TotalEpisodes,
                                    LastPlayedDate: s.LastPlayedDate,
                                    DateAdd: s.DateAdd,
                                    Rating: s.Rating,
                                    TotalSizeMB: s.TotalSizeMB,
                                    Categories: s.Categories))
                .ToArray();
        }

        public override async Task<Tutorial> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Tutorials
                        .Include(i => i.Folders)
                        .ThenInclude(f => f.Episodes)
                        .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        public async Task UpdateEpisodeAsync(Episode episode, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(episode);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
