﻿using dataneo.TutorialLibs.Domain.Tutorials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class TutorialConfiguration : IEntityTypeConfiguration<Tutorial>
    {
        public void Configure(EntityTypeBuilder<Tutorial> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.OwnsOne(o => o.BasePath,
                            b => b.Property(p => p.Source)
                                    .IsRequired()
                                    .HasMaxLength(255));

            builder.Property(p => p.AddDate)
                    .IsRequired();

            builder.Property(p => p.ModifiedTime)
                    .IsRequired();

            builder.Property(p => p.Rating)
                    .IsRequired();

            builder.HasMany(m => m.Folders)
                   .WithOne()
                   .HasForeignKey(f => f.ParentTutorialId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Categories)
                   .WithMany(p => p.Tutorials)
                   .UsingEntity(j => j.ToTable("CategoriesTutorials"));
        }
    }
}
