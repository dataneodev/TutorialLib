using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Settings;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class CategoryManager : BindableBase
    {
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;
        private readonly ISettingManager _settingManager;

        public CategoryManager(ICategoryRespositoryAsync categoryRespositoryAsync, ISettingManager settingManager)
        {
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));
            this._settingManager = Guard.Against.Null(settingManager, nameof(settingManager));
        }

        private IEnumerable<CategoryMenuItem> _categories;
        public IEnumerable<CategoryMenuItem> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(); }
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await this._categoryRespositoryAsync.ListAllAsync();
            var savedSelectedCategories = await this._settingManager.GetValueIntCollectionAsync(SettingDefinition.LastCategory);
            var categoryMenuItems = CreateCategoryMenuItem(categories)
                                        .Concat(GetDefaultCategory())
                                        .ToArray();
            if (savedSelectedCategories.HasValue)
                SetStatusToCategories(categoryMenuItems, savedSelectedCategories.GetValueOrThrow());
            this.Categories = categoryMenuItems;
        }

        public Task SaveCategoriesSelectAsync()
            => this._settingManager.SetIntCollectionValueAsync(
                SettingDefinition.LastCategory,
                this.Categories.Where(w => w.IsChecked && w.FilterByCategory())
                               .Select(s => s.GetCategoryId().Value)
                               .Concat(this.Categories.Where(w => w.IsChecked && w.FilterByNone()).Take(1).Select(s => 0))
                               .ToArray());

        public IEnumerable<Category> GetFilteredCategories()
            => this.Categories?.Where(w => w.IsChecked && w.FilterByCategory())
                               .Select(s => s.GetCategory().GetValueOrThrow()) ?? Enumerable.Empty<Category>();

        public bool IsFilterByNoCategory()
            => this.Categories?.Any(a => a.FilterByNone()) ?? false;

        private void SetStatusToCategories(IReadOnlyList<CategoryMenuItem> categoryMenuItems, IReadOnlyList<int> enabledCategoriesIds)
        {
            foreach (var category in categoryMenuItems)
            {
                category.IsChecked = category.GetCategoryId().HasValue &&
                                     enabledCategoriesIds.Contains(category.GetCategoryId().Value);

                if (category.GetCategoryId().HasNoValue && enabledCategoriesIds.Contains(0))
                    category.IsChecked = true;
            }
        }

        private IEnumerable<CategoryMenuItem> CreateCategoryMenuItem(IReadOnlyList<Category> categories)
            => categories
                .OrderBy(o => o.Name)
                .Select(category => new CategoryMenuItem(category));

        private IEnumerable<CategoryMenuItem> GetDefaultCategory()
        {
            yield return CategoryMenuItem.CreateForNoCategory();
        }
    }
}
