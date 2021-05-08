using dataneo.TutorialLibs.Domain.Enums;

namespace TutorialsLib
{
    public class VideoItem
    {
        public string Name { get; init; }
        public VideoItemLocationType LocationOnList { get; init; }
        public VideoWatchStatus WatchStatus { get; init; }
        public FolderItem Folder { get; init; }
    }
}
