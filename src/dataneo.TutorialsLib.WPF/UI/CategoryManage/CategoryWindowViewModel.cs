using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Categories;
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
        private readonly IDialogService _dialogService;

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

        public CategoryWindowViewModel(IRegionManager regionManager,
                                       ICategoryRespositoryAsync categoryRespositoryAsync,
                                       IDialogService dialogService) : base(regionManager)
        {
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));
            this._dialogService = Guard.Against.Null(dialogService, nameof(dialogService));

            this.AddCategoryCommand = new Command(AddCategoryCommandImpl);
            this.UpdateCategoryCommand = new Command(UpdateCategoryCommandImpl);
            this.DeleteCategoryCommand = new Command(DeleteCategoryCommandImpl);
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public async void OnDialogOpened(IDialogParameters parameters)
            => await Result
                 .Try(() => LoadCategoriesAsync())
                 .OnFailure(ShowError);

        private async void DeleteCategoryCommandImpl()
        {
            if ((this.SelectedCategory?.Id ?? 0) == 0)
                return;

            await Result
               .Try(() => this._categoryRespositoryAsync.DeleteAsync(this.SelectedCategory))
               .OnSuccessTry(() => LoadCategoriesAsync())
               .OnFailure(ShowError);
        }

        private async void UpdateCategoryCommandImpl()
        {
            if ((this.SelectedCategory?.Id ?? 0) == 0)
                return;

            await new UpdateCategory(this._categoryRespositoryAsync)
                .UpdateCategoryNameAsync(this.SelectedCategory, this.CategoryName)
                .OnSuccessTry(() => LoadCategoriesAsync())
                .OnFailure(ShowError);
        }

        private async void AddCategoryCommandImpl()
        => await Category.Create(this.CategoryName)
                .Bind(category => new AddCategory(this._categoryRespositoryAsync)
                                        .AddNewCategoryAsync(category))
                .OnSuccessTry(() => LoadCategoriesAsync())
                .OnFailure(ShowError);

        private async Task LoadCategoriesAsync()
        {
            this.Categories = await this._categoryRespositoryAsync.ListAllAsync();
        }

        private void ShowError(string error)
            => this._dialogService.ShowDialog(
                        nameof(DialogWindow),
                        new DialogParameters($"Message={error}"),
                        null);
    }
}
