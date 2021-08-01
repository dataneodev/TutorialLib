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
        private readonly int _tutorialPlayerId;
        private readonly Window _windowHandle;

        private IReadOnlyList<object> videoItems;
        public IReadOnlyList<object> VideoItems
        {
            get { return videoItems; }
            set { videoItems = value; Notify(); }
        }

        public ICommand ClickedOnEpisodeOrFolderCommand { get; }
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
            this._tutorialPlayerId = Guard.Against.NegativeOrZero(tutorialPlayerId, nameof(tutorialPlayerId));
            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeOrFolderCommand = new Command<int>(ClickedOnEpisodeOrFolderCommandImpl);
            this._windowHandle = Guard.Against.Null(windowHandle, nameof(windowHandle));
        }

        private void CurrentVideoEndedCommandImpl()
        {
            this.CurrentMediaPath = @"Z:\District 9 2009 ITA-ENG BRRip 720p x264-HD4ME\District.9.2009.ITA-ENG.BRRip.720p.x264-HD4ME.mkv";
        }

        private void ClickedOnEpisodeOrFolderCommandImpl(int episodeId)
        {

        }

        public async Task LoadAsync()
        {
            var loadPlayedList = new VideoItemsCreator();
            (await loadPlayedList.GetAsync(this._tutorialPlayerId))
                .Tap(playedList => this.VideoItems = playedList)
                .OnFailure(error => ErrorWindow.ShowError(this._windowHandle, error));
        }

        //private void NewMethod()
        //{
        //    VideoItems.Add(new FolderItem
        //    {
        //        Name = "Start",
        //        WatchStatus = VideoWatchStatus.Watched
        //    });

        //    VideoItems.Add(
        //        new VideoItem
        //        {
        //            Name = "Test tytułu 1",
        //            LocationOnList = VideoItemLocationType.FirstElement
        //        });

        //    VideoItems.Add(new VideoItem
        //    {
        //        Name = "Test tytułu 2",
        //        LocationOnList = VideoItemLocationType.InnerElement
        //    });

        //    VideoItems.Add(new VideoItem
        //    {
        //        Name = "Test tytułu 3",
        //        LocationOnList = VideoItemLocationType.LastElement
        //    });

        //    VideoItems.Add(new FolderItem
        //    {
        //        Name = "Folder test",
        //        WatchStatus = VideoWatchStatus.Watched
        //    });

        //    VideoItems.Add(
        //        new VideoItem
        //        {
        //            Name = "Test tytułu 1",
        //            LocationOnList = VideoItemLocationType.FirstElement,
        //            WatchStatus = VideoWatchStatus.Watched
        //        });

        //    VideoItems.Add(new VideoItem
        //    {
        //        Name = "Test tytułu 2",
        //        LocationOnList = VideoItemLocationType.InnerElement,
        //        WatchStatus = VideoWatchStatus.InProgress
        //    });

        //    VideoItems.Add(new VideoItem
        //    {
        //        Name = "Test tytułu 3",
        //        LocationOnList = VideoItemLocationType.LastElement,
        //        WatchStatus = VideoWatchStatus.NotWatched
        //    });

        //    VideoItems.Add(new FolderItem
        //    {
        //        Name = "Folder test",
        //        WatchStatus = VideoWatchStatus.NotWatched
        //    });
        //}

        public void Test()
        {
            // this.CurrentMediaPath = @"F:\Teledyski\Karolina Stanisławczyk - Cliché (official music video) (1080p_25fps_AV1-128kbit_AAC)_KjQYmiGcBKA.mp4";

        }
    }
}
