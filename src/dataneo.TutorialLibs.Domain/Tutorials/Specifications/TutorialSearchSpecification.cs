using Ardalis.GuardClauses;
using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Categories;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials.Specifications
{
    public class TutorialSearchSpecification : Specification<Tutorial>
    {
        public TutorialSearchSpecification FilterByCategories(IEnumerable<Category> categories, bool filterWithNoCategory = true)
        {
            Guard.Against.Null(categories, nameof(categories));
            var categoriesIdsSet = categories.Select(s => s.Id).ToHashSet();
            if (categoriesIdsSet.Count == 0)
                return this;

            if (filterWithNoCategory)
            {
                Query.Where(tutorial => tutorial.Categories.Any() == false ||
                                        tutorial.Categories.Any(category => categoriesIdsSet.Contains(category.Id)));
                return this;
            }

            Query.Where(tutorial => tutorial.Categories.Any(category => categoriesIdsSet.Contains(category.Id)));
            return this;
        }

        public TutorialSearchSpecification Page(short page, short recordOnPage)
        {
            Guard.Against.NegativeOrZero(page, nameof(page));
            Guard.Against.NegativeOrZero(recordOnPage, nameof(recordOnPage));
            Query.Skip((page - 1) * recordOnPage)
                 .Take(recordOnPage);
            return this;
        }

        public TutorialSearchSpecification TutorialTitleSearch(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return this;

            Query.Where(w => w.Name.Contains(title));

            return this;
        }

        public TutorialSearchSpecification OrderByTitle()
        {
            Query.OrderBy(o => o.Name);
            return this;
        }

        public TutorialSearchSpecification OrderByDateAdd()
        {
            Query.OrderByDescending(o => o.AddDate);
            return this;
        }

        public TutorialSearchSpecification OrderByRating()
        {
            Query.OrderByDescending(o => o.Rating);
            return this;
        }

        public TutorialSearchSpecification OrderByDateWatch()
        {
            Query.OrderByDescending(o => o.Folders.Max(m => m.Episodes.Max(m2 => m2.LastPlayedDate)));
            return this;
        }
    }
}
