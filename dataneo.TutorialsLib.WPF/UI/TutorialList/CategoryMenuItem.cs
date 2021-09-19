using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Categories;
using System;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class CategoryMenuItem
    {
        public enum FilterType : byte
        {
            ByCategory,
            ByNoCataegory,
            All
        }

        private readonly Category _category;
        public FilterType Filter { get; private set; }

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
                Filter = FilterType.ByNoCataegory
            };

        public static CategoryMenuItem CreateForAll()
            => new CategoryMenuItem
            {
                Filter = FilterType.All
            };

        public override string ToString()
            => this.Filter switch
            {
                FilterType.ByCategory => this._category.Name,
                FilterType.All => Translation.UI.ALL,
                FilterType.ByNoCataegory => Translation.UI.NOCATEGORY,
                _ => throw new InvalidOperationException(),
            };
    }
}
