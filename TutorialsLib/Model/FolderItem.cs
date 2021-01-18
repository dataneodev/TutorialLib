using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
