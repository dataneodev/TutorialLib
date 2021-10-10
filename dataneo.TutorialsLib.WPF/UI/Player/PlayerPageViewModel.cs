using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using dataneo.TutorialLibs.WPF.Events;
using dataneo.TutorialLibs.WPF.UI.Player.Services;
using dataneo.TutorialLibs.WPF.UI.TutorialList;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.Player
{
    internal class PlayerPageViewModel : BaseViewModel
    {
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly IDialogService _dialogService;

        private QueueManager _queueManager;
        private IReadOnlyList<object> videoItems;
        public IReadOnlyList<object> VideoItems
        {
            get { return videoItems; }
            set { videoItems = value; RaisePropertyChanged(); }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        public ICommand ClickedOnEpisodeCommand { get; }
        public ICommand CurrentVideoEndedCommand { get; }
        public ICommand NextEpisodeCommand { get; }
        public ICommand PrevEpisodeCommand { get; }
        public ICommand GoBackCommand { get; }

        private PlayFileParameter currentMediaPath;
        public PlayFileParameter CurrentMediaPath
        {
            get { return currentMediaPath; }
            set
            {
                currentMediaPath = value;
                RaisePropertyChanged();
            }
        }

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                RaisePropertyChanged();
                SetEpsiodePosition(value);
            }
        }

        private string folderTitle;
        public string FolderTitle
        {
            get { return folderTitle; }
            set { folderTitle = value; RaisePropertyChanged(); }
        }

        private string episodeTitle;

        public string EpisodeTitle
        {
            get { return episodeTitle; }
            set { episodeTitle = value; RaisePropertyChanged(); }
        }

        public PlayerPageViewModel(IRegionManager regionManager,
                                   IDialogService dialogService,
                                   IEventAggregator eventAggregator,
                                   ITutorialRespositoryAsync tutorialRespositoryAsync) : base(regionManager)
        {
            this._tutorialRespositoryAsync = Guard.Against.Null(tutorialRespositoryAsync, nameof(tutorialRespositoryAsync));
            this._dialogService = Guard.Against.Null(dialogService, nameof(dialogService));

            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeCommand = new Command<int>(ClickedOnEpisodeCommandImpl);
            this.NextEpisodeCommand = new Command(NextEpisodeCommandImpl);
            this.PrevEpisodeCommand = new Command(PrevEpisodeCommandImpl);
            this.GoBackCommand = new Command(GoBackCommandImpl);

            eventAggregator.GetEvent<CloseAppEvent>().Subscribe(AppClose);
        }

        public async override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var tutorialId = navigationContext.Parameters["tutorialId"];
            await Result
                .Try(() => LoadAsync((int)tutorialId))
                .OnFailure(ShowError);
        }

        private void _queueManager_BeginPlayFile(PlayFileParameter playFileParameter)
        {
            this.CurrentMediaPath = playFileParameter;
            this.Caption = playFileParameter.TutorialTitle;
            this.FolderTitle = playFileParameter.FolderTitle;
            this.EpisodeTitle = playFileParameter.EpisodeTitle;
            this.SelectedItem = playFileParameter.Item;
        }

        private void CurrentVideoEndedCommandImpl()
            => this._queueManager?.CurrentPlayedEpisodeHasEnded();

        private void ClickedOnEpisodeCommandImpl(int episodeId)
            => this._queueManager?.UserRequestEpisodePlay(episodeId);

        private void SetEpsiodePosition(int position)
            => this._queueManager?.SetPlayedEpisodePositionAsync(position);

        private void PrevEpisodeCommandImpl()
            => this._queueManager?.PlayPrevEpisodeAsync();

        private void NextEpisodeCommandImpl()
            => this._queueManager?.PlayNextEpisodeAsync();

        private async void GoBackCommandImpl()
        {
            await Result
                .Try(() => EndWorkAsync())
                .OnFailure(ShowError);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(TutorialListPage));
        }

        private async void AppClose()
            => await Result.Try(EndWorkAsync);

        private async Task LoadAsync(int tutorialId)
        {
            Guard.Against.NegativeOrZero(tutorialId, nameof(tutorialId));
            var videoItemsCreator = new VideoItemsCreatorEngine(this._tutorialRespositoryAsync);

            (await videoItemsCreator.LoadAndCreate(tutorialId))
                .OnFailure(ShowError)
                .Tap(result =>
                {
                    this._queueManager = new QueueManager(this._tutorialRespositoryAsync, result);
                    this._queueManager.BeginPlayFile += _queueManager_BeginPlayFile;
                    this.VideoItems = result.AllItems;
                    this._queueManager.StartupPlay();
                });
        }

        private async Task EndWorkAsync()
        {
            if (this._queueManager is null)
                return;
            await this._queueManager
                            .EndWorkAsync()
                            .ConfigureAwait(false);
        }

        private void ShowError(string error)
            => this._dialogService.ShowDialog(
                        nameof(DialogWindow),
                        new DialogParameters($"Message={error}"),
                        null);
    }
}
