using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using dataneo.TutorialLibs.WPF.UI.Player.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI
{
    internal class PlayerWindowVM : BaseViewModel
    {
        private readonly int _tutorialId;
        private readonly Window _windowHandle;
        private QueueManager _queueManager;

        private IReadOnlyList<object> videoItems;
        public IReadOnlyList<object> VideoItems
        {
            get { return videoItems; }
            set { videoItems = value; Notify(); }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; Notify(); }
        }

        public ICommand ClickedOnEpisodeCommand { get; }
        public ICommand CurrentVideoEndedCommand { get; }
        public ICommand NextEpisodeCommand { get; }
        public ICommand PrevEpisodeCommand { get; }
        public ICommand FullscreenToggleCommand { get; }

        private PlayFileParameter currentMediaPath;
        public PlayFileParameter CurrentMediaPath
        {
            get { return currentMediaPath; }
            set
            {
                currentMediaPath = value;
                Notify();
            }
        }

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                Notify();
                SetEpsiodePosition(value);
            }
        }

        private string folderTitle;
        public string FolderTitle
        {
            get { return folderTitle; }
            set { folderTitle = value; Notify(); }
        }

        private string episodeTitle;
        public string EpisodeTitle
        {
            get { return episodeTitle; }
            set { episodeTitle = value; Notify(); }
        }

        public PlayerWindowVM(Window windowHandle, int tutorialPlayerId)
        {
            this._windowHandle = Guard.Against.Null(windowHandle, nameof(windowHandle));
            this._tutorialId = Guard.Against.NegativeOrZero(tutorialPlayerId, nameof(tutorialPlayerId));
            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeCommand = new Command<int>(ClickedOnEpisodeCommandImpl);
            this.FullscreenToggleCommand = new Command(FullscreenToggleCommandImpl);
            this.NextEpisodeCommand = new Command(NextEpisodeCommandImpl);
            this.PrevEpisodeCommand = new Command(PrevEpisodeCommandImpl);
        }

        WindowState toggleFullscreenOldState;
        private void FullscreenToggleCommandImpl()
        {
            if (this._windowHandle.WindowState != WindowState.Maximized)
            {
                toggleFullscreenOldState = this._windowHandle.WindowState;
                this._windowHandle.WindowState = WindowState.Maximized;
                this._windowHandle.Visibility = Visibility.Collapsed;
                this._windowHandle.WindowStyle = WindowStyle.None;
                this._windowHandle.ResizeMode = ResizeMode.NoResize;
                this._windowHandle.Visibility = Visibility.Visible;
                this._windowHandle.Activate();
            }
            else
            {
                this._windowHandle.WindowState = toggleFullscreenOldState;
                this._windowHandle.WindowStyle = WindowStyle.SingleBorderWindow;
                this._windowHandle.ResizeMode = ResizeMode.CanResize;
            }
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

        public async Task LoadAsync()
        {
            using var repo = new TutorialRespositoryAsync();
            var videoItemsCreator = new VideoItemsCreatorEngine(repo);

            (await videoItemsCreator.LoadAndCreate(this._tutorialId))
                .OnFailure(error => ErrorWindow.ShowError(this._windowHandle, error))
                .Tap(result =>
                {
                    this._queueManager = new QueueManager(result);
                    this._queueManager.BeginPlayFile += _queueManager_BeginPlayFile;
                    this.VideoItems = result.AllItems;
                    this._queueManager.StartupPlay();
                });
        }

        public async Task EndWorkAsync()
            => await this._queueManager
                            .EndWorkAsync()
                            .ConfigureAwait(false);
    }
}
