using dataneo.TutorialLibs.Domain.Tutorials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.FolderPath)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.Order)
                    .IsRequired();

            builder.HasMany(m => m.Episodes)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
