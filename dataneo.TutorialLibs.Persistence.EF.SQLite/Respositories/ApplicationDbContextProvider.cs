using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Interfaces;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories
{
    public class ApplicationDbContextProvider : IApplicationDbContext
    {
        public ApplicationDbContext GetApplicationDbContext()
        {
            return new ApplicationDbContext();
        }
    }
}
