using Ardalis.Specification;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.Domain.Tutorials.Specifications;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class SpecificationBuilder
    {
        private readonly TutorialSearchSpecification _tutorialSearchSpecification = new TutorialSearchSpecification();

        public SpecificationBuilder FilterByCategories(IEnumerable<Category> categories, bool filterWithNoCategory = true)
        {
            this._tutorialSearchSpecification.FilterByCategories(categories, filterWithNoCategory);
            return this;
        }

        public SpecificationBuilder Page(short page, short recordOnPage)
        {
            this._tutorialSearchSpecification.Page(page, recordOnPage);
            return this;
        }

        public SpecificationBuilder TutorialTitleSearch(string title)
        {
            this._tutorialSearchSpecification.TutorialTitleSearch(title);
            return this;
        }

        public SpecificationBuilder OrderBy(TutorialsOrderType tutorialsOrderType)
        {
            switch (tutorialsOrderType)
            {
                case TutorialsOrderType.ByTitle:
                    this._tutorialSearchSpecification.OrderByTitle();
                    break;
                case TutorialsOrderType.ByDateAdd:
                    this._tutorialSearchSpecification.OrderByDateAdd();
                    break;
                case TutorialsOrderType.ByLastVisit:
                    this._tutorialSearchSpecification.OrderByDateWatch();
                    break;
                case TutorialsOrderType.ByRating:
                    this._tutorialSearchSpecification.OrderByRating();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return this;
        }

        public ISpecification<Tutorial> GetSpecification()
            => this._tutorialSearchSpecification;
    }
}
