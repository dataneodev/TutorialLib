using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Specifications;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.CategoryManage
{
    internal sealed class CategoryVM : BaseViewModel
    {
        private IEnumerable<Category> categories;
        public IEnumerable<Category> Categories
        {
            get { return categories; }
            set { categories = value; Notify(); }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                Notify();
                Notify(nameof(EditExistingCategory));
                if (value is not null)
                {
                    this.CategoryName = value.Name;
                }
            }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; Notify(); }
        }

        public bool EditExistingCategory => this.SelectedCategory is not null;

        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryVM()
        {
            this.AddCategoryCommand = new Command(AddCategoryCommandImpl);
            this.UpdateCategoryCommand = new Command(UpdateCategoryCommandImpl);
            this.DeleteCategoryCommand = new Command(DeleteCategoryCommandImpl);
        }

        private void DeleteCategoryCommandImpl()
        {

        }

        private void UpdateCategoryCommandImpl()
        {

        }

        private void AddCategoryCommandImpl()
        {

        }

        public async Task LoadCategoriesAsync()
        {
            using var repo = new CattegoryRespositoryAsync();
            var retriveSpec = new CategoryRetrive();
            this.Categories = await repo.ListAsync(retriveSpec);
        }
    }
}
