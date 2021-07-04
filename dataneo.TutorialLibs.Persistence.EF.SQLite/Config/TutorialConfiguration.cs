using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class TutorialConfiguration : IEntityTypeConfiguration<Tutorial>
    {
        public void Configure(EntityTypeBuilder<Tutorial> builder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, long>(
               v => v.ToBinary(),
               v => DateTime.FromBinary(v));

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.OwnsOne(o => o.BasePath,
                            b => b.Property(p => p.Source)
                                    .IsRequired()
                                    .HasMaxLength(255));

            builder.Property(p => p.AddDate)
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

            builder.Property(p => p.ModifiedTime)
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

            builder.Property(p => p.Rating)
                    .IsRequired();

            builder.HasMany(m => m.Folders)
                   .WithOne()
                   .HasForeignKey(f => f.ParentTutorialId)
                   .IsRequired();
        }
    }
}
