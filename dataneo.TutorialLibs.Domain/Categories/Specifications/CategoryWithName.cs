using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class CategoryWithName : Specification<Category>
    {
        public CategoryWithName(Category category)
        {
            Query.Where(w => w.Id != category.Id && w.Name.Equals(category.Name));
        }
    }
}
