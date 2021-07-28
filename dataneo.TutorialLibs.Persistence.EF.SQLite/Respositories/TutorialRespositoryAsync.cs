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

        public async Task<IReadOnlyList<TutorialHeaderDto>> GetTutorialHeadersDtoByIdAsync(CancellationToken cancellationToken = default)
        {
            return await this._dbContext.Tutorials
                    .Select(s =>
                     new TutorialHeaderDto
                     {
                         Id = s.Id,
                         Name = s.Name,
                         DateAdd = s.AddDate,
                         Rating = s.Rating,
                         //  PlayedEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count(w => w.PlayedTime > w.File.PlayTime * 0.92)),
                         //  LastPlayedDate = s.Folders.Max(m => m.Episodes.Select(se => se.LastPlayedDate)).Max(),
                         //  TimePlayed = s.Folders.Sum(w => w.Episodes.Sum(se => se.PlayedTime.TotalSeconds)),
                         TotalEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count()),
                         TotalSizeMB = (float)s.Folders.Sum(w => w.Episodes.Sum(se => se.File.FileSize / 1048576f)),
                     })
                    .ToListAsync();
        }

        public async Task<Maybe<TutorialHeaderDto>> GetTutorialHeaderDtoByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var tutorial = await this._dbContext.Tutorials
                    .Where(w => w.Id == id)
                    .Take(1)
                    .Select(s =>
                     new TutorialHeaderDto
                     {
                         Id = s.Id,
                         Name = s.Name,
                         DateAdd = s.AddDate,
                         Rating = s.Rating,
                         PlayedEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count(w => w.PlayedTime > w.File.PlayTime * 0.92)),
                         LastPlayedDate = s.Folders.Max(m => m.Episodes.Select(se => se.LastPlayedDate)).Max(),
                         //TimePlayed = s.Folders.Sum(w => w.Episodes.Sum(se => se.PlayedTime)),
                         TotalEpisodes = (short)s.Folders.Sum(w => w.Episodes.Count()),
                         TotalSizeMB = (float)s.Folders.Sum(w => w.Episodes.Sum(se => se.File.FileSize / 1048576f)),
                     })
                    .FirstOrDefaultAsync();
            return Maybe<TutorialHeaderDto>.From(tutorial);
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}
