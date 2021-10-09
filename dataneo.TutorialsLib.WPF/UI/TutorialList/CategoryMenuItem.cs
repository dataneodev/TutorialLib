using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Categories;
using Prism.Mvvm;
using System;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class CategoryMenuItem : BindableBase
    {
        public enum FilterType : byte
        {
            ByCategory,
            ByNoCategory,
        }

        private readonly Category _category;
        public FilterType Filter { get; private set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }

        private CategoryMenuItem() { }

        public CategoryMenuItem(Category category)
        {
            Guard.Against.Null(category, nameof(category));
            this._category = category;
            this.Filter = FilterType.ByCategory;
        }

        public static CategoryMenuItem CreateForNoCategory()
            => new CategoryMenuItem
            {
                Filter = FilterType.ByNoCategory
            };

        public override string ToString()
            => this.Filter switch
            {
                FilterType.ByCategory => this._category.Name,
                FilterType.ByNoCategory => Translation.UI.NOCATEGORY,
                _ => throw new InvalidOperationException(),
            };

        public bool FilterByNone()
            => this.IsChecked && this.Filter == FilterType.ByNoCategory;

        public bool FilterByCategory()
            => this.IsChecked && this.Filter == FilterType.ByCategory;

        public Maybe<int> GetCategoryId()
            => (this._category is null) ? Maybe<int>.None : Maybe<int>.From(this._category.Id);

        public bool IsEqualCategoryId(int categoryId)
            => this.Filter == FilterType.ByCategory && this._category.Id == categoryId;

        public Maybe<Category> GetCategory() => this._category;
    }
}
