using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal class PlayerWindowVM : BaseViewModel
    {
        public IList<object> VideoItems { get; set; } = new List<object>();


        public ICommand ClickedOnEpisodeOrFolder;

        private string currentMediaPath = @"F:\Teledyski\Karolina Stanisławczyk - Cliché (official music video) (1080p_25fps_AV1-128kbit_AAC)_KjQYmiGcBKA.mp4";
        public string CurrentMediaPath
        {
            get { return currentMediaPath; }
            set { currentMediaPath = value; Notify(); }
        }

        public PlayerWindowVM(Guid playedTutorialId)
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
