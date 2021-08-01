using dataneo.TutorialLibs.Domain.Enums;
using System;

namespace dataneo.TutorialsLib.WPF.UI
{
    public class FolderItem
    {
        public int FolderId { get; init; }
        public short Position { get; set; }
        public string Name { get; init; }
        public TimeSpan FolderPlayTime { get; init; }

        public VideoWatchStatus WatchStatus { get; set; }
    }
}
