using dataneo.TutorialLibs.Persistence.EF.SQLite.Context;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Interfaces
{
    public interface IApplicationDbContext
    {
        ApplicationDbContext GetApplicationDbContext();
    }
}
