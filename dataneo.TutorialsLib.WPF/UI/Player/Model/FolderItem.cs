using dataneo.TutorialLibs.Domain.Enums;
using System;

namespace TutorialsLib
{
    public class FolderItem
    {
        public short Position { get; set; }
        public string Name { get; set; }
        public TimeSpan FolderPlayTime { get; set; }

        public VideoWatchStatus WatchStatus { get; set; }
    }
}
