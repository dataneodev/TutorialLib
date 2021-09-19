using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.Tutorials;
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
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;

        private IEnumerable<TutorialHeaderDto> tutorials;
        public IEnumerable<TutorialHeaderDto> Tutorials
        {
            get { return tutorials; }
            set { tutorials = value; RaisePropertyChanged(); }
        }

        private TutorialsOrderType selectedTutorialsOrderType = TutorialsOrderType.ByDateAdd;
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

        private IEnumerable<CategoryMenuItem> _categories = new List<CategoryMenuItem>()
        {
            CategoryMenuItem.CreateForAll(),
            CategoryMenuItem.CreateForNoCategory()
        };
        private readonly IDialogService _dialogService;

        public IEnumerable<CategoryMenuItem> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(); }
        }

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }
        public ICommand FilterByCategory { get; }
        public ICommand ShowCategoryManagerCommand { get; }

        public TutorialListPageViewModel(IRegionManager regionManager,
                                         IDialogService dialogService,
                                         ITutorialRespositoryAsync tutorialRespositoryAsync) : base(regionManager)
        {
            this.RatingChangedCommand = new Command<ValueTuple<int, RatingStars>>(RatingChangedCommandImpl);
            this.PlayTutorialCommand = new Command<int>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImplAsync);
            this.FilterByCategory = new Command<CategoryMenuItem>(FilterByCategoryImplAsync);
            this.ShowCategoryManagerCommand = new Command(ShowCategoryManagerCommandImpl);
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._dialogService = Guard.Against.Null(dialogService, nameof(dialogService));
        }

        public async override void OnNavigatedTo(NavigationContext navigationContext)
            => await Result
                .Try(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType))
                .OnFailure(error => ErrorWindow.ShowError(error));

        private async void AddTutorialCommandImplAsync()
            => await Result
               .Try(() => new AddNewTutorialAction(this._tutorialRespositoryAsync).AddAsync(), e => e.Message)
               .Bind(r => r)
               .OnSuccessTry(() => LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType))
               .OnFailure(error => ErrorWindow.ShowError(error));

        private async void PlayTutorialCommandImpl(int tutorialId)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(tutorialId), tutorialId);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(PlayerPage), parameters);
        }

        private async void RatingChangedCommandImpl(ValueTuple<int, RatingStars> tutorialIdAndRating)
            => await Result
                .Success(tutorialIdAndRating)
                .Map(input => ChangeTutorialRatingAction.ChangeAsync(this._tutorialRespositoryAsync, input.Item1, input.Item2))
                .Bind(r => r)
                .OnFailure(error => ErrorWindow.ShowError(error));

        private void FilterByCategoryImplAsync(CategoryMenuItem obj)
        {

        }

        private void ShowCategoryManagerCommandImpl()
        {
            this._dialogService.ShowDialog(nameof(CategoryWindow),
                async callback =>
                {
                    await LoadTutorialsDtoAsync(this.SelectedTutorialsOrderType);

                });
        }

        private async Task LoadTutorialsDtoAsync(TutorialsOrderType tutorialsOrderType)
        {
            var tutorialHeaders = await this._tutorialRespositoryAsync.GetAllTutorialHeadersDtoAsync();
            SetNewTutorialsHeader(tutorialHeaders, tutorialsOrderType);
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
