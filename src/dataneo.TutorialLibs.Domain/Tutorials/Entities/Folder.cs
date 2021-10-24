using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public sealed class Folder : BaseEntity
    {
        private const int MinFolderName = 1;
        public short Order { get; private set; }
        public string Name { get; private set; }
        public string FolderPath { get; private set; }
        private readonly List<Episode> _episodes = new List<Episode>();
        public IReadOnlyList<Episode> Episodes => new ReadOnlyCollection<Episode>(this._episodes);
        public bool IsRootFolder => string.IsNullOrWhiteSpace(FolderPath);

        private Folder() { }

        public static Result<Folder> Create(string folderPath, IReadOnlyList<Episode> episodes)
            => Create(folderPath, folderPath, episodes);

        public static Result<Folder> Create(string folderPath, string folderName, IReadOnlyList<Episode> episodes)
        {
            if ((episodes?.Count ?? 0) < 1)
                return Result.Failure<Folder>(Errors.NO_EPISODE);

            var folderPathTrimmed = folderName?.Trim();

            if (String.IsNullOrWhiteSpace(folderPathTrimmed))
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            if (folderPathTrimmed.Length < MinFolderName)
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            var invalidChars = Path.GetInvalidPathChars();
            if (invalidChars.Any(c => folderPath.Contains(c)))
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            var folder = new Folder
            {
                FolderPath = folderPath,
                Name = folderPathTrimmed,
            };
            folder._episodes.AddRange(episodes);
            return folder;
        }

        public void SetOrder(short order)
        {
            if (order < 0)
                throw new ArgumentException(nameof(order));
            Order = order;
        }

        public TimeSpan GetFolderPlayedTime()
            => TimeSpan.FromSeconds(this.Episodes.Sum(s => s.File.PlayTime.TotalSeconds));

        public VideoWatchStatus GetFolderStatus()
            => this.Episodes.Aggregate(
                    VideoWatchStatus.Watched,
                    (aggregate, episode) => episode.Status < aggregate ? episode.Status : aggregate);

        public IEnumerable<Episode> GetOrderedEpisode()
            => this.Episodes.OrderBy(o => o.Order);
    }
}
