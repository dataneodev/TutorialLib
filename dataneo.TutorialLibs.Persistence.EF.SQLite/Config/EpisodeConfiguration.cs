﻿using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            var timeSpanConverter = new ValueConverter<TimeSpan, long>(
               v => v.Ticks,
               v => TimeSpan.FromTicks(v));

            var dateTimeConverter = new ValueConverter<DateTime, long>(
               v => v.ToBinary(),
               v => DateTime.FromBinary(v));

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Order)
                    .IsRequired();

            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.PlayedTime)
                    .IsRequired()
                    .HasConversion(timeSpanConverter);

            builder.Property(p => p.LastPlayedDate)
                    .IsRequired();

            builder.Property(p => p.DateAdd)
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

            builder.OwnsOne(o =>
                o.File,
                b =>
                {
                    b.Property(p => p.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                    b.Property(p => p.DateCreated)
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

                    b.Property(p => p.DateModified)
                    .IsRequired()
                    .HasConversion(dateTimeConverter);

                    b.Property(p => p.FileSize)
                    .IsRequired();

                    b.Property(p => p.PlayTime)
                     .HasConversion(timeSpanConverter)
                     .IsRequired();
                });
        }
    }
}
