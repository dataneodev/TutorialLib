using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Domain.Translation;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System;
using System.IO;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Episode : BaseEntity
    {
        public const byte EpisonNameMinLength = 2;
        public const byte UnWatchPercentage = 8;
        public const byte WatchPercentage = 92;

        public Guid ParentFolderId { get; private set; }
        public short Order { get; set; }
        public string Name { get; private set; }
        public EpisodeFile File { get; private set; }
        public TimeSpan PlayedTime { get; set; }
        public DateTime LastPlayedDate { get; private set; }
        public DateTime DateAdd { get; private set; }


        public VideoWatchStatus Status => GetWatchStatus();

        private Episode() { }

        public static Result<Episode> Create(Guid parentFolderId, EpisodeFile episodeFile, DateTime dateAdd)
        {
            Guard.Against.Null(episodeFile, nameof(episodeFile));
            if (parentFolderId == Guid.Empty)
                return Result.Failure<Episode>(Errors.EMPTY_PARENT_FOLDER_ID);

            return new Episode
            {
                Name = Path.GetFileNameWithoutExtension(episodeFile.FileName),
                DateAdd = dateAdd,
                ParentFolderId = parentFolderId,
                File = episodeFile
            };
        }

        private VideoWatchStatus GetWatchStatus()
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

        public void SetPlayedTime(TimeSpan playedTime)
        {
            if (playedTime < TimeSpan.Zero)
                throw new InvalidOperationException();

            if (playedTime > this.File.PlayTime)
                throw new InvalidOperationException();

            this.PlayedTime = playedTime;
        }

        public Result SetNewName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure(Errors.EPISODE_NAME_EMPTY);

            var newnameTrimed = newName.Trim();

            if (newnameTrimed.Length < EpisonNameMinLength)
                return Result.Failure(Errors.EPISODE_NAME_TO_SHORT);

            this.Name = newnameTrimed;

            return Result.Success();
        }
    }
}
