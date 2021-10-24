using dataneo.TutorialLibs.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasMany(m => m.SettingItems)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
