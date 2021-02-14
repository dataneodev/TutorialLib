using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
