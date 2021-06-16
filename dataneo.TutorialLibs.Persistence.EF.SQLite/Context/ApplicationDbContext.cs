using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TestDatabase.db", options =>
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
