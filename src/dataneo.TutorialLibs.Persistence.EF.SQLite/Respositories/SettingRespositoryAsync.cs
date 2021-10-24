using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Settings;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class SettingRespositoryAsync : EfRepository<Setting>, ISettingRespositoryAsync
    {
        public SettingRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public override async Task<Setting> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Settings
                        .Include(i => i.SettingItems)
                        .FirstOrDefaultAsync(f => f.Id == id, cancellationToken)
                        .ConfigureAwait(false);
    }
}
