using dataneo.TutorialLibs.Domain.Enums;
using System;

namespace dataneo.TutorialsLib.WPF.UI
{
    public class VideoItem
    {
        public int EpisodeId { get; init; }
        public string Name { get; init; }
        public VideoItemLocationType LocationOnList { get; init; }
        public VideoWatchStatus WatchStatus { get; set; }
        public TimeSpan EpisodePlayTime { get; init; }
    }
}
