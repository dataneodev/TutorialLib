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

            builder.Ignore(i => i.PlayedTime);

            builder.Property(p => p.PlayedTimeSecond)
                   .IsRequired();

            builder.Property(p => p.LastPlayedDate)
                    .IsRequired();

            builder.Property(p => p.DateAdd)
                    .IsRequired();

            builder.OwnsOne(o =>
                o.File,
                b =>
                {
                    b.Property(p => p.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                    b.Property(p => p.DateCreated)
                    .IsRequired();

                    b.Property(p => p.DateModified)
                    .IsRequired();

                    b.Property(p => p.FileSize)
                    .IsRequired();

                    b.Ignore(i => i.PlayTime);

                    b.Property(p => p.PlayTimeSecond)
                     .IsRequired();
                });
        }
    }
}
