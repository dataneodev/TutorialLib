using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
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
        public TutorialRespositoryAsync() : base(new ApplicationDbContext())
        { }

        public TutorialRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public async Task<IReadOnlyList<TutorialHeaderDto>> GetAllTutorialHeadersDtoAsync(CancellationToken cancellationToken = default)
        {
            var watchProgressFractor = Episode.WatchPercentage / 100f;
            var result = await this._dbContext.Tutorials
                .Select(s =>
                    new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        DateAdd = s.AddDate,
                        Rating = s.Rating,
                        LastPlayedDate = s.Folders.Min(m => m.Episodes.Min(se => se.LastPlayedDate)),
                        TotalTime = s.Folders.Sum(w => w.Episodes.Sum(se => se.File.PlayTimeSecond)),
                        TimePlayed = s.Folders.Sum(w => w.Episodes.Sum(se => se.PlayedTimeSecond)),
                        PlayedEpisodes = (short)s.Folders.Sum(w =>
                                            w.Episodes.Count(w => w.PlayedTimeSecond > w.File.PlayTimeSecond * watchProgressFractor)),
                        TotalEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count()),
                        TotalSizeMB = (float)s.Folders.Sum(w => w.Episodes.Sum(se => se.File.FileSize / 1048576f)),
                    })
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.Select(s => new TutorialHeaderDto
            {
                Id = s.Id,
                Name = s.Name,
                DateAdd = s.DateAdd,
                Rating = s.Rating,
                LastPlayedDate = s.LastPlayedDate,
                TotalTime = TimeSpan.FromSeconds(s.TotalTime),
                TimePlayed = TimeSpan.FromSeconds(s.TimePlayed),
                PlayedEpisodes = s.PlayedEpisodes,
                TotalEpisodes = s.TotalEpisodes,
                TotalSizeMB = s.TotalSizeMB
            }).ToArray();
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
