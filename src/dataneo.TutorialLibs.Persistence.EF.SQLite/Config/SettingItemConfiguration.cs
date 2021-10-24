using dataneo.TutorialLibs.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class SettingItemConfiguration : IEntityTypeConfiguration<SettingItem>
    {
        public void Configure(EntityTypeBuilder<SettingItem> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasIndex(i => i.Name).IsUnique();
            builder.Property(p => p.Name)
                    .HasMaxLength(SettingItem.MaxNameLength)
                    .IsUnicode(false)
                    .IsRequired();

            builder.Property(p => p.Value)
                    .IsUnicode(false)
                    .HasMaxLength(SettingItem.MaxValueLength);
        }
    }
}
