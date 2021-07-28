using dataneo.TutorialLibs.Domain.Enums;
using System.Collections.Generic;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal class PlayerWindowVM : BaseViewModel
    {
        private readonly int _tutorialPlayerId;

        public IList<object> VideoItems { get; set; } = new List<object>();

        public ICommand ClickedOnEpisodeOrFolderCommand { get; }
        public ICommand CurrentVideoEndedCommand { get; }

        private string currentMediaPath;
        public string CurrentMediaPath
        {
            get { return currentMediaPath; }
            set
            {
                currentMediaPath = value;
                Notify(nameof(CurrentMediaPath));
            }
        }

        public PlayerWindowVM(int tutorialPlayerId)
        {
            this._tutorialPlayerId = tutorialPlayerId;
            this.CurrentVideoEndedCommand = new Command(CurrentVideoEndedCommandImpl);
            this.ClickedOnEpisodeOrFolderCommand = new Command(ClickedOnEpisodeOrFolderCommandImpl);
        }

        private void CurrentVideoEndedCommandImpl()
        {
            this.CurrentMediaPath = @"Z:\District 9 2009 ITA-ENG BRRip 720p x264-HD4ME\District.9.2009.ITA-ENG.BRRip.720p.x264-HD4ME.mkv";
        }

        private void ClickedOnEpisodeOrFolderCommandImpl()
        {

        }

        public void Load()
        {
            NewMethod();

            Test();
        }

        private void NewMethod()
        {
            VideoItems.Add(new FolderItem
            {
                Name = "Start",
                WatchStatus = VideoWatchStatus.Watched
            });

            VideoItems.Add(
                new VideoItem
                {
                    Name = "Test tytułu 1",
                    LocationOnList = VideoItemLocationType.FirstElement
                });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 2",
                LocationOnList = VideoItemLocationType.InnerElement
            });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 3",
                LocationOnList = VideoItemLocationType.LastElement
            });

            VideoItems.Add(new FolderItem
            {
                Name = "Folder test",
                WatchStatus = VideoWatchStatus.Watched
            });

            VideoItems.Add(
                new VideoItem
                {
                    Name = "Test tytułu 1",
                    LocationOnList = VideoItemLocationType.FirstElement,
                    WatchStatus = VideoWatchStatus.Watched
                });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 2",
                LocationOnList = VideoItemLocationType.InnerElement,
                WatchStatus = VideoWatchStatus.InProgress
            });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 3",
                LocationOnList = VideoItemLocationType.LastElement,
                WatchStatus = VideoWatchStatus.NotWatched
            });

            VideoItems.Add(new FolderItem
            {
                Name = "Folder test",
                WatchStatus = VideoWatchStatus.NotWatched
            });
        }

        public void Test()
        {
            this.CurrentMediaPath = @"F:\Teledyski\Karolina Stanisławczyk - Cliché (official music video) (1080p_25fps_AV1-128kbit_AAC)_KjQYmiGcBKA.mp4";

        }
    }
}
