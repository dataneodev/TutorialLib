using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Entities;

namespace dataneo.TutorialLibs.Domain.Specifications
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
