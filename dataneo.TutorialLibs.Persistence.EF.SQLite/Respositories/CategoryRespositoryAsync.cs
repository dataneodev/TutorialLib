using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using System;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class CattegoryRespositoryAsync : EfRepository<Category>, ICategoryRespositoryAsync, IDisposable
    {
        public CattegoryRespositoryAsync(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

    }
}
