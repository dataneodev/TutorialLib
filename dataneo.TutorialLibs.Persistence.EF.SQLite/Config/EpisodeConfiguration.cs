using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Order)
                    .IsRequired();

            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.PlayedTime)
                    .IsRequired();

            builder.Property(p => p.LastPlayedDate)
                    .IsRequired();

            builder.Property(p => p.DateAdd)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

            builder.OwnsOne(o =>
                o.File,
                b =>
                {
                    b.Property(p => p.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                    b.Property(p => p.DateCreated)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                    b.Property(p => p.DateModified)
                    .IsRequired()
                    .ValueGeneratedOnAddOrUpdate();

                    b.Property(p => p.FileSize)
                    .IsRequired();

                    b.Property(p => p.PlayTime)
                    .IsRequired();
                });
        }
    }
}
