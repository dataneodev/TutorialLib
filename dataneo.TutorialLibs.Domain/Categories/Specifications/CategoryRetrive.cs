using Ardalis.Specification;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class CategoryRetrive : Specification<Category>
    {
        public CategoryRetrive()
        {
            Query.AsNoTracking()
                .Include(i => i.Tutorials);
        }
    }
}
