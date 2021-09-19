using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class CategoryWithNameSpecification : Specification<Category>
    {
        public CategoryWithNameSpecification(Category category)
        {
            Query.Where(w => w.Id != category.Id && w.Name.Equals(category.Name));
        }
    }
}
