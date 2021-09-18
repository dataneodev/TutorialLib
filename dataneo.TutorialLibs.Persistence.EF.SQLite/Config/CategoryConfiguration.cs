using dataneo.TutorialLibs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataneo.TutorialLibs.Persistence.EF.SQLite.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasIndex(i => i.Name).IsUnique();
            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(Category.MaxCategoryName);
        }
    }
}
