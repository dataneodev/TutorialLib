using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.IO;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public sealed class Episode : BaseEntity
    {
        public const byte EpisonNameMinLength = 2;
        public const byte UnWatchPercentage = 10;
        public const byte WatchPercentage = 96;
        public short Order { get; private set; }
        public string Name { get; private set; }
        public EpisodeFile File { get; private set; }
        public TimeSpan PlayedTime { get; private set; }
        public DateTime LastPlayedDate { get; private set; }
        public DateTime DateAdd { get; private set; }

        public VideoWatchStatus Status => GetWatchStatus();
        public double PlayedTimeSecond
        {
            get
            {
                return this.PlayedTime.TotalSeconds;
            }
            private set
            {
                this.PlayedTime = TimeSpan.FromSeconds(value);
            }
        }

        private Episode() { }

        public static Result<Episode> Create(EpisodeFile episodeFile, DateTime now)
        {
            Guard.Against.Null(episodeFile, nameof(episodeFile));

            if (now == DateTime.MinValue)
                return Result.Failure<Episode>(Errors.INVALID_DATE);

            return new Episode
            {
                Name = Path.GetFileNameWithoutExtension(episodeFile.FileName),
                DateAdd = now,
                File = episodeFile
            };
        }

        private VideoWatchStatus GetWatchStatus()
        {
            if (this.File == null)
                throw new NullReferenceException(nameof(File));

            return GetWatchStatus(this.PlayedTime, this.File.PlayTime);
        }

        public static VideoWatchStatus GetWatchStatus(TimeSpan playedTime, TimeSpan totalEpisodeTime)
        {
            var playRatio = (playedTime / totalEpisodeTime) * 100;
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

            return Path.Combine(tutorial.BasePath.Source, folder.FolderPath, this.File.FileName);
        }

        public void SetPlayedTime(TimeSpan playedTime, DateTime now)
        {
            if (playedTime < TimeSpan.Zero)
                throw new InvalidOperationException();

            if (playedTime > this.File.PlayTime)
                throw new InvalidOperationException();

            if (!IsWatched())
                this.PlayedTime = playedTime;

            this.LastPlayedDate = now;
        }

        public void SetAsWatched()
            => this.PlayedTime = this.File.PlayTime;

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

        public void SetOrder(short order)
        {
            if (order < 0)
                throw new ArgumentException(nameof(order));
            Order = order;
        }

        public bool IsWatched()
            => this.Status == VideoWatchStatus.Watched;

    }
}
