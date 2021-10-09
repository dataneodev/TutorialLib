using Ardalis.GuardClauses;
using Ardalis.Specification;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.Domain.Tutorials.Services;
using dataneo.TutorialLibs.Domain.Tutorials.Specifications;
using dataneo.TutorialLibs.WPF.Actions;
using dataneo.TutorialLibs.WPF.Comparers;
using dataneo.TutorialLibs.WPF.UI.CategoryManage;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using dataneo.TutorialLibs.WPF.UI.Player;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    internal sealed class TutorialListPageViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;

        private IEnumerable<TutorialHeaderDto> tutorials;
        public IEnumerable<TutorialHeaderDto> Tutorials
        {
            get { return tutorials; }
            set { tutorials = value; RaisePropertyChanged(); }
        }

        private TutorialHeaderDto selectedTutorial;
        public TutorialHeaderDto SelectedTutorial
        {
            get { return selectedTutorial; }
            set { selectedTutorial = value; RaisePropertyChanged(); }
        }

        private TutorialsOrderType selectedTutorialsOrderType = TutorialsOrderType.ByLastVisit;
        public TutorialsOrderType SelectedTutorialsOrderType
        {
            get { return selectedTutorialsOrderType; }
            set
            {
                selectedTutorialsOrderType = value;
                RaisePropertyChanged();
                SetNewTutorialsHeader(Tutorials, value);
            }
        }

        private IEnumerable<CategoryMenuItem> _categories;
        public IEnumerable<CategoryMenuItem> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(); }
        }

        private IEnumerable<CategoryMenuItem> _tutorialCategories;
        public IEnumerable<CategoryMenuItem> TutorialCategories
        {
            get { return _tutorialCategories; }
            set { _tutorialCategories = value; RaisePropertyChanged(); }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand ShowCategoryManagerCommand { get; }
        public ICommand ShowTutorialCategoriesCommand { get; }
        public ICommand TutorialCategoriesChangedCommand { get; }

        public TutorialListPageViewModel(IRegionManager regionManager,
                                         IDialogService dialogService,
                                         ITutorialRespositoryAsync tutorialRespositoryAsync,
                                         ICategoryRespositoryAsync categoryRespositoryAsync) : base(regionManager)
        {
            this._dialogService = Guard.Against.Null(dialogService, nameof(dialogService));
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));

            this.RatingChangedCommand = new Command<ValueTuple<int, RatingStars>>(RatingChangedCommandImplAsync);
            this.PlayTutorialCommand = new Command<int>(PlayTutorialCommandImplAsync);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
            this.FilterByCategoryCommand = new Command(FilterByCategoryCommandImplAsync);
            this.ShowCategoryManagerCommand = new Command(ShowCategoryManagerCommandImpl);
            this.ShowTutorialCategoriesCommand = new Command(ShowTutorialCategoriesCommandImpl);
            this.TutorialCategoriesChangedCommand = new Command(TutorialCategoriesChangedCommandImpl);
        }

        public async override void OnNavigatedTo(NavigationContext navigationContext)
                  => await Result
                      .Try(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType, GetSpecificationAccToFilterSelect()))
                      .OnSuccessTry(() => LoadCategoriesAsync())
                      .OnFailure(error => ErrorWindow.ShowError(error));

        private async void AddTutorialCommandImplAsync()
            => await Result
               .Try(() => new AddNewTutorialAction(this._tutorialRespositoryAsync).AddAsync(), e => e.Message)
               .Bind(r => r)
               .OnSuccessTry(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType, GetSpecificationAccToFilterSelect()))
               .OnFailure(error => ErrorWindow.ShowError(error));

        private async void PlayTutorialCommandImplAsync(int tutorialId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(tutorialId), tutorialId);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(PlayerPage), parameters);
        }

        private async void RatingChangedCommandImplAsync(ValueTuple<int, RatingStars> tutorialIdAndRating)
            => await Result
                .Success(tutorialIdAndRating)
                .Map(input => ChangeTutorialRatingAction.ChangeAsync(this._tutorialRespositoryAsync, input.Item1, input.Item2))
                .Bind(r => r)
                .OnFailure(error => ErrorWindow.ShowError(error));

        private async void FilterByCategoryCommandImplAsync()
            => await Result
                .Try(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType, GetSpecificationAccToFilterSelect()))
                .OnFailure(error => ErrorWindow.ShowError(error));

        private void ShowCategoryManagerCommandImpl()
            => this._dialogService.ShowDialog(nameof(CategoryWindow),
                    async callback => await LoadCategoriesAndTutorialsonSelectionChangeAsync());

        private async void ShowTutorialCategoriesCommandImpl()
        {
            if (this.SelectedTutorial is null)
                return;

            this.TutorialCategories = GetCategoryMenuItem(this.SelectedTutorial, this.Categories);
        }

        private IReadOnlyList<CategoryMenuItem> GetCategoryMenuItem(TutorialHeaderDto tutorialHeaderDto,
                                                                    IEnumerable<CategoryMenuItem> categoryMenuItems)
        {
            var categories = categoryMenuItems.Select(s => s.GetCategory())
                                               .Where(w => w.HasValue)
                                               .Select(s => new CategoryMenuItem(s.Value))
                                               .ToArray();

            if (tutorialHeaderDto.Categories is null)
                return categories;

            var toChecked = categories
                .Join(
                    tutorialHeaderDto.Categories,
                    f => f.GetCategoryId(),
                    s => s.Id,
                    (f, s) => f);

            foreach (var category in toChecked)
            {
                category.IsChecked = true;
            }
            return categories;
        }

        private async void TutorialCategoriesChangedCommandImpl()
        {
            if (this.SelectedTutorial is null)
                return;
            var selectedTutorial = this.SelectedTutorial;
            var newTutorialCategories = this.TutorialCategories.Where(w => w.IsChecked)
                                                    .Select(S => S.GetCategory().Value)
                                                    .ToArray();

            UpdateTutorialDtoCategories(newTutorialCategories, selectedTutorial);
            await UpdateTutorialCategoriesAsync(newTutorialCategories, selectedTutorial);
        }

        private async void UpdateTutorialDtoCategories(IReadOnlyList<Category> newTutorialCategories, TutorialHeaderDto tutorialHeaderDto)
        {
            var newTutorialDto = tutorialHeaderDto with { Categories = newTutorialCategories };
            this.Tutorials = this.Tutorials.Select(s => s == tutorialHeaderDto ? newTutorialDto : s)
                                           .ToArray();
        }

        private async Task UpdateTutorialCategoriesAsync(IReadOnlyList<Category> newTutorialCategories, TutorialHeaderDto tutorialHeaderDto)
        {
            var updateCategory = new UpdateTutorial(this._tutorialRespositoryAsync);
            (await updateCategory.UpdateTutorialCategoriesAsync(tutorialHeaderDto.Id, newTutorialCategories))
                .OnFailure(error => ErrorWindow.ShowError(error));
        }

        private async Task LoadCategoriesAndTutorialsonSelectionChangeAsync()
        {
            var categories = await this._categoryRespositoryAsync.ListAllAsync();
            var categoriesSelectionChange = IsSelectedCategoryChange(categories);
            SetCategories(categories);
            if (categoriesSelectionChange)
                await LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType, GetSpecificationAccToFilterSelect());
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await this._categoryRespositoryAsync.ListAllAsync();
            SetCategories(categories);
        }

        private bool IsSelectedCategoryChange(IReadOnlyList<Category> newCategory)
        {
            if (this.Categories.All(a => !a.IsChecked))
                return false;

            return this.Categories.Any(w => w.FilterByCategory() &&
                                            !newCategory.Any(a => w.IsEqualCategoryId(a.Id)));
        }

        private void SetCategories(IReadOnlyList<Category> categories)
        {
            var newCategories = ProcessCategories(categories)
                                .Concat(GetDefaultCategory())
                                .ToArray();
            MergeCheckedWithNew(this.Categories, newCategories);
            this.Categories = newCategories;
        }

        private void MergeCheckedWithNew(IEnumerable<CategoryMenuItem> oldCategories,
                                         IReadOnlyList<CategoryMenuItem> newCategories)
        {
            if (oldCategories is null)
                return;

            foreach (var merge in oldCategories
                                .Where(w => w.FilterByCategory())
                                .Join(newCategories.Where(w => w.Filter == CategoryMenuItem.FilterType.ByCategory),
                                    f => f.GetCategoryId().Value,
                                    s => s.GetCategoryId().Value,
                                    (f, s) => new { f, s }))
            {
                merge.s.IsChecked = merge.f.IsChecked;
            }
        }

        private IEnumerable<CategoryMenuItem> ProcessCategories(IReadOnlyList<Category> categories)
            => categories
                .OrderBy(o => o.Name)
                .Select(category => new CategoryMenuItem(category));

        private IEnumerable<CategoryMenuItem> GetDefaultCategory()
        {
            yield return CategoryMenuItem.CreateForNoCategory();
        }

        private async Task LoadTutorialsDtoAsync(TutorialsOrderType tutorialsOrderType, ISpecification<Tutorial> specification)
        {
            var tutorialHeaders = await this._tutorialRespositoryAsync.GetAllTutorialHeadersDtoAsync(specification);
            SetNewTutorialsHeader(tutorialHeaders, tutorialsOrderType);
        }

        private ISpecification<Tutorial> GetSpecificationAccToFilterSelect()
        {
            if (this.Categories?.All(a => !a.IsChecked) ?? true)
                return new AllTutorialsSpecification();

            var noCategoryFilter = this.Categories.Any(a => a.FilterByNone());

            return new FilterByCategoryIdsSpecification(
                this.Categories.Where(w => w.FilterByCategory())
                               .Select(s => s.GetCategoryId().Value),
                noCategoryFilter);
        }

        private void SetNewTutorialsHeader(IEnumerable<TutorialHeaderDto> tutorialHeaders,
                                           TutorialsOrderType tutorialsOrderType)
        {
            var comparer = TutorialsOrderComparerFactory.GetComparer(tutorialsOrderType);
            this.Tutorials = tutorialHeaders.OrderBy(o => o, comparer)
                                            .ToArray();
        }
    }
}
