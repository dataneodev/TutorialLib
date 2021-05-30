using dataneo.TutorialLibs.Domain.Enums;
using System;

namespace dataneo.TutorialsLib.WPF.UI
{
    public class VideoItem
    {
        public string Name { get; init; }
        public VideoItemLocationType LocationOnList { get; init; }
        public VideoWatchStatus WatchStatus { get; init; }
        public TimeSpan EpisodePlayTime { get; set; }
        public FolderItem Folder { get; init; }
    }
}
