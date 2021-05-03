using System;

namespace TutorialLibs.Application.Model
{
    public struct EpisodeFile
    {
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
