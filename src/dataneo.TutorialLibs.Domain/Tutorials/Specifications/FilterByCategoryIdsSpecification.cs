using Ardalis.Specification;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials.Specifications
{
    public class FilterByCategoryIdsSpecification : Specification<Tutorial>
    {
        public FilterByCategoryIdsSpecification(IEnumerable<int> categoryIds, bool noCategory)
        {
            var categoriesidsSet = categoryIds.ToHashSet();
            if (noCategory)
            {
                Query.Where(tutorial => tutorial.Categories.Any() == false ||
                                        tutorial.Categories.Any(category => categoriesidsSet.Contains(category.Id)));
                return;
            }

            Query.Where(tutorial => tutorial.Categories.Any(category => categoriesidsSet.Contains(category.Id)));
        }
    }
}
