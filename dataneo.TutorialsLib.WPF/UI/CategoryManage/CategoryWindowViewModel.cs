using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Interfaces.Respositories;
using dataneo.TutorialLibs.Domain.Specifications;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.CategoryManage
{
    public sealed class CategoryWindowViewModel : BaseViewModel, IDialogAware
    {
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;

        private string _title = Translation.UI.CATEGORYWINDOW;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private IEnumerable<Category> categories;
        public IEnumerable<Category> Categories
        {
            get { return categories; }
            set { categories = value; RaisePropertyChanged(); }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(EditExistingCategory));
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
            set { categoryName = value; RaisePropertyChanged(); }
        }

        public bool EditExistingCategory => this.SelectedCategory is not null;

        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public event Action<IDialogResult> RequestClose;

        public CategoryWindowViewModel(IRegionManager regionManager, ICategoryRespositoryAsync categoryRespositoryAsync) : base(regionManager)
        {
            this.AddCategoryCommand = new Command(AddCategoryCommandImpl);
            this.UpdateCategoryCommand = new Command(UpdateCategoryCommandImpl);
            this.DeleteCategoryCommand = new Command(DeleteCategoryCommandImpl);
            this._categoryRespositoryAsync = categoryRespositoryAsync;
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public async void OnDialogOpened(IDialogParameters parameters)
            => await Result
                 .Try(() => LoadCategoriesAsync())
                 .OnFailure(error => ErrorWindow.ShowError(error));

        private void DeleteCategoryCommandImpl()
        {

        }

        private void UpdateCategoryCommandImpl()
        {

        }

        private void AddCategoryCommandImpl()
        {

        }

        private async Task LoadCategoriesAsync()
        {
            var retriveSpec = new CategoryRetrive();
            this.Categories = await this._categoryRespositoryAsync.ListAsync(retriveSpec);
        }
    }
}
