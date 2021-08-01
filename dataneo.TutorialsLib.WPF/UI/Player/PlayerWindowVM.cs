using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using dataneo.TutorialsLib.WPF.UI.Dialogs;
using dataneo.TutorialsLib.WPF.UI.Player.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal class PlayerWindowVM : BaseViewModel
    {
        private readonly Window _windowHandle;
        private QueueManager _queueManager;

        private IReadOnlyList<object> videoItems;
        public IReadOnlyList<object> VideoItems
        {
            get { return videoItems; }
            set { videoItems = value; Notify(); }
        }

        public ICommand ClickedOnEpisodeCommand { get; }
        public ICommand CurrentVideoEndedCommand { get; }

        private string currentMediaPath;
        public string CurrentMediaPath
        {
            get { return currentMediaPath; }
            set
            {
                currentMediaPath = value;
                Notify();
            }
        }

        public PlayerWindowVM(Window windowHandle, int tutorialPlayerId)
        {
            this._windowHandle = Guard.Against.Null(windowHandle, nameof(windowHandle));

            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeCommand = new Command<int>(ClickedOnEpisodeCommandImpl);

            LoadAsync(tutorialPlayerId);
        }

        private void _queueManager_BeginPlayFile(string filePath)
            => this.CurrentMediaPath = filePath;

        private void CurrentVideoEndedCommandImpl()
            => this._queueManager?.CurrentPlayedEpisodeHasEnded();

        private void ClickedOnEpisodeCommandImpl(int episodeId)
            => this._queueManager?.UserRequestEpisodePlay(episodeId);

        private async Task LoadAsync(int tutorialId)
        {
            using var repo = new TutorialRespositoryAsync();
            var videoItemsCreator = new VideoItemsCreatorEngine(repo);

            (await videoItemsCreator.LoadAndCreate(tutorialId))
                .OnFailure(error => ErrorWindow.ShowError(this._windowHandle, error))
                .Tap(result =>
                {
                    this._queueManager = new QueueManager(result);
                    this._queueManager.BeginPlayFile += _queueManager_BeginPlayFile;
                    this.VideoItems = result.AllItemsProcessed;
                    this._queueManager.StartupPlay();
                });
        }
    }
}
