using Ardalis.Specification;
using System;

namespace dataneo.TutorialLibs.Domain.Categories
{
    public class CategoryWithName : Specification<Category>
    {
        public CategoryWithName(string categoryName)
        {
            Query.Where(w => w.Name.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
