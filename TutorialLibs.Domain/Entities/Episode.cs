using System;
using TutorialLibs.Domain.Enums;
using TutorialLibs.SharedKernel.Common;

namespace TutorialLibs.Domain.Entities
{
    public class Episode : BaseEntity
    {
        public short Lp { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
        public VideoWatchStatus Status { get; set; }
        public TimeSpan Duraction { get; set; }
        public TimeSpan PlayedTime { get; set; }
        public long FileSize { get; set; }
    }
}
