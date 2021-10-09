using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Interfaces;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class CattegoryRespositoryAsync : EfRepositoryDetached<Category>, ICategoryRespositoryAsync
    {
        public CattegoryRespositoryAsync(IApplicationDbContext applicationDbContext) : base(applicationDbContext)
        { }
    }
}
