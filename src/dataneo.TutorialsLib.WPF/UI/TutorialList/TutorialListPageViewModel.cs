using Ardalis.GuardClauses;
using Ardalis.Specification;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Settings;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.WPF.Actions;
using dataneo.TutorialLibs.WPF.UI.CategoryManage;
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
        private const short TutorialOnPage = 40;

        private readonly IDialogService _dialogService;
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly ICategoryRespositoryAsync _categoryRespositoryAsync;
        private readonly IAddTutorial _addTutorial;
        private readonly ISettingManager _settingManager;

        public readonly CategoryManager CategoriesManager;

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
            }
        }

        private IEnumerable<CategoryMenuItem> _tutorialCategories;
        public IEnumerable<CategoryMenuItem> TutorialCategories
        {
            get { return _tutorialCategories; }
            set { _tutorialCategories = value; RaisePropertyChanged(); }
        }

        private short page;
        public short Page
        {
            get { return page; }
            set
            {
                if (value < 1)
                {
                    page = 1;
                    return;
                }

                if (value > TotalPage)
                {
                    page = TotalPage;
                    return;
                }
                page = value;
                RaisePropertyChanged();
            }
        }

        private short totalPage = 1;
        public short TotalPage
        {
            get { return totalPage; }
            set
            {
                if (value < 1)
                {
                    totalPage = 1;
                    return;
                }
                totalPage = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand ShowCategoryManagerCommand { get; }
        public ICommand SetTutorialAsWatchedCommand { get; }
        public ICommand SetTutorialAsUnWatchedCommand { get; }
        public ICommand DeleteTutorialCommand { get; }
        public ICommand ShowTutorialCategoriesCommand { get; }
        public ICommand TutorialCategoriesChangedCommand { get; }

        public TutorialListPageViewModel(IRegionManager regionManager,
                                         IDialogService dialogService,
                                         IAddTutorial addTutorial,
                                         ITutorialRespositoryAsync tutorialRespositoryAsync,
                                         ICategoryRespositoryAsync categoryRespositoryAsync,
                                         ISettingManager settingManager) : base(regionManager)
        {
            this._dialogService = Guard.Against.Null(dialogService, nameof(dialogService));
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._categoryRespositoryAsync = Guard.Against.Null(categoryRespositoryAsync, nameof(categoryRespositoryAsync));
            this._settingManager = Guard.Against.Null(settingManager, nameof(settingManager));
            this._addTutorial = Guard.Against.Null(addTutorial, nameof(addTutorial));

            this.CategoriesManager = new CategoryManager(categoryRespositoryAsync, settingManager);

            this.RatingChangedCommand = new Command<ValueTuple<int, RatingStars>>(RatingChangedCommandImplAsync);
            this.PlayTutorialCommand = new Command<int>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
            this.FilterByCategoryCommand = new Command(FilterByCategoryCommandImplAsync);
            this.ShowCategoryManagerCommand = new Command(ShowCategoryManagerCommandImpl);
            this.DeleteTutorialCommand = new Command(DeleteTutorialCommandImpl);
            this.SetTutorialAsWatchedCommand = new Command(SetTutorialAsWatchedCommandImpl);
            this.SetTutorialAsUnWatchedCommand = new Command(SetTutorialAsUnWatchedCommandImpl);
            this.ShowTutorialCategoriesCommand = new Command(ShowTutorialCategoriesCommandImpl);
            this.TutorialCategoriesChangedCommand = new Command(TutorialCategoriesChangedCommandImpl);
        }

        public async override void OnNavigatedTo(NavigationContext navigationContext)
            => await Result
                .Try(this.CategoriesManager.LoadCategoriesAsync)
                .OnSuccessTry(LoadTutorialsDtoAsync)
                .OnFailure(error => ShowError(error));

        private async void AddTutorialCommandImplAsync()
            => await Result
               .Try(() => new AddNewTutorialAction(this._addTutorial).AddAsync(), e => e.Message)
               .Bind(r => r)
               .OnSuccessTry(LoadTutorialsDtoAsync)
               .OnFailure(error => ShowError(error));

        private void PlayTutorialCommandImpl(int tutorialId)
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
                .OnFailure(error => ShowError(error));

        private async void FilterByCategoryCommandImplAsync()
            => await Result
                .Try(this.CategoriesManager.SaveCategoriesSelectAsync)
                .OnSuccessTry(LoadTutorialsDtoAsync)
                .OnFailure(error => ShowError(error));

        private void ShowCategoryManagerCommandImpl()
            => this._dialogService.ShowDialog(nameof(CategoryWindow),
                    async callback => await LoadCategoriesAndTutorialsonSelectionChangeAsync());

        private void SetTutorialAsUnWatchedCommandImpl() { }

        private void SetTutorialAsWatchedCommandImpl() { }

        private void DeleteTutorialCommandImpl() { }

        private void ShowTutorialCategoriesCommandImpl()
        {
            if (this.SelectedTutorial is null)
                return;

            this.TutorialCategories = GetCategoryMenuItem(this.SelectedTutorial, this.CategoriesManager.Categories);
        }

        private IReadOnlyList<CategoryMenuItem> GetCategoryMenuItem(TutorialHeaderDto tutorialHeaderDto,
                                                                    IEnumerable<CategoryMenuItem> categoryMenuItems)
        {
            var categories = categoryMenuItems.Select(s => s.GetCategory())
                                               .Where(w => w.HasValue)
                                               .Select(s => new CategoryMenuItem(s.GetValueOrThrow()))
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
                                                    .Select(S => S.GetCategory().GetValueOrThrow())
                                                    .ToArray();

            UpdateTutorialDtoCategories(newTutorialCategories, selectedTutorial);
            await UpdateTutorialCategoriesAsync(newTutorialCategories, selectedTutorial);
        }

        private void UpdateTutorialDtoCategories(IReadOnlyList<Category> newTutorialCategories, TutorialHeaderDto tutorialHeaderDto)
        {
            var newTutorialDto = tutorialHeaderDto with { Categories = newTutorialCategories };
            this.Tutorials = this.Tutorials.Select(s => s == tutorialHeaderDto ? newTutorialDto : s)
                                           .ToArray();
        }

        private async Task UpdateTutorialCategoriesAsync(IReadOnlyList<Category> newTutorialCategories, TutorialHeaderDto tutorialHeaderDto)
        {
            var updateCategory = new Domain.Tutorials.Services.UpdateCategory(this._tutorialRespositoryAsync);
            (await updateCategory.UpdateTutorialCategoriesAsync(tutorialHeaderDto.Id, newTutorialCategories))
                .OnFailure(error => ShowError(error));
        }

        private async Task LoadCategoriesAndTutorialsonSelectionChangeAsync()
        {
            await this.CategoriesManager.LoadCategoriesAsync();
            await LoadTutorialsDtoAsync();
        }

        private async Task LoadTutorialsDtoAsync()
        {
            var specificationCount = GetSpecificationAccToFilterSelect();
            var totalRecords = await this._tutorialRespositoryAsync.CountAsync(specificationCount);
            this.TotalPage = (short)Math.Ceiling(totalRecords / (double)TutorialOnPage);
            this.Page = 1;
            var specification = GetSpecificationAccToFilterSelect(this.Page);
            this.Tutorials = await this._tutorialRespositoryAsync.GetAllTutorialHeadersDtoAsync(specification);
        }

        private ISpecification<Tutorial> GetSpecificationAccToFilterSelect(short? page = null)
        {
            var specBuilder = new SpecificationBuilder();
            specBuilder
                .FilterByCategories(this.CategoriesManager.GetFilteredCategories(),
                                    this.CategoriesManager.IsFilterByNoCategory());

            if (page.HasValue)
            {
                specBuilder.Page(page.Value, TutorialOnPage)
                           .OrderBy(this.SelectedTutorialsOrderType);
            }


            return specBuilder.GetSpecification();
        }

        private void ShowError(string error)
            => this._dialogService.ShowDialog(
                        nameof(DialogWindow),
                        new DialogParameters($"Message={error}"),
                        null);
    }
}
