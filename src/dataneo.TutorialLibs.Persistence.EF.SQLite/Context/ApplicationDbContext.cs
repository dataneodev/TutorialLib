using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Settings;
using dataneo.TutorialLibs.Domain.Tutorials;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Context
{
    public class ApplicationDbContext : DbContext
    {
        public const string DatabaseName = "TutorialsLibDB.db";

        public DbSet<Tutorial> Tutorials { get; private set; }
        public DbSet<Category> Categories { get; private set; }
        public DbSet<Folder> Folders { get; private set; }
        public DbSet<Episode> Episodes { get; private set; }
        public DbSet<Setting> Settings { get; private set; }
        public DbSet<SettingItem> SettingItems { get; private set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabaseName}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
