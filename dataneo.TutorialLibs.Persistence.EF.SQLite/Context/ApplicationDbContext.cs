using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Folder> Folders { get; set; }
    }
}
