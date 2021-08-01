using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
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
        private readonly QueueManager _queueManager;

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
            this._queueManager = new QueueManager(tutorialPlayerId);
            this._queueManager.BeginPlayFile += _queueManager_BeginPlayFile;
            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeCommand = new Command<int>(ClickedOnEpisodeCommandImpl);
            this._windowHandle = Guard.Against.Null(windowHandle, nameof(windowHandle));
        }

        private void _queueManager_BeginPlayFile(string filePath)
            => this.CurrentMediaPath = filePath;

        private void CurrentVideoEndedCommandImpl()
            => this._queueManager.CurrentPlayedEpisodeHasEnded();

        private void ClickedOnEpisodeCommandImpl(int episodeId)
            => this._queueManager.UserRequestEpisodePlay(episodeId);

        public async Task LoadAsync()
        {
            (await this._queueManager.LoadVideoItemsAsync())
                .OnFailure(error => ErrorWindow.ShowError(this._windowHandle, error))
                .Tap(videoList => this.VideoItems = videoList)
                .Tap(() => this._queueManager.StartupPlay());
        }
    }
}
