using Ardalis.GuardClauses;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System;
using System.IO;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Episode : BaseEntity
    {
        public const byte UnWatchPercentage = 8;
        public const byte WatchPercentage = 92;

        public int ParentFolderId { get; set; }
        public short Order { get; set; }
        public string Name { get; set; }
        public EpisodeFile File { get; set; }
        public VideoWatchStatus Status { get; }
        public TimeSpan PlayedTime { get; set; }
        public DateTime DateAdd { get; set; }

        public VideoWatchStatus GetWatchStatus()
        {
            if (this.File == null)
                throw new NullReferenceException(nameof(File));

            var playRatio = (PlayedTime / File.PlayTime) * 100;
            if (playRatio < UnWatchPercentage)
                return VideoWatchStatus.NotWatched;

            if (playRatio > WatchPercentage)
                return VideoWatchStatus.Watched;

            return VideoWatchStatus.InProgress;
        }

        public string GetFilePath(Tutorial tutorial, Folder folder)
        {
            Guard.Against.Null(tutorial, nameof(tutorial));
            Guard.Against.Null(folder, nameof(folder));

            return Path.Combine(tutorial.BasePath, folder.FolderName, this.File.FileName);
        }
    }
}
