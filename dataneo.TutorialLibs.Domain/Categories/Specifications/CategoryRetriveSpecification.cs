using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class CategoryRetriveSpecification : Specification<Category>
    {
        public CategoryRetriveSpecification()
        {
            Query.AsNoTracking()
                .Include(i => i.Tutorials);
        }
    }
}
