using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Categories;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public sealed class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public DirectoryPath BasePath { get; private set; }
        public RatingStars Rating { get; private set; }
        public IReadOnlyList<Folder> Folders { get; private set; }

        private readonly List<Category> _categories = new List<Category>();
        public IReadOnlyList<Category> Categories => new ReadOnlyCollection<Category>(this._categories);

        public DateTime AddDate { get; private set; }
        public DateTime ModifiedTime { get; private set; }

        public void SetRating(RatingStars ratingStars)
            => this.Rating = ratingStars;

        private Tutorial() { }

        public static Result<Tutorial> Create(
                        string name,
                        DirectoryPath basePath,
                        IReadOnlyList<Folder> folders,
                        DateTime dateTimeNow)
        {
            if (String.IsNullOrWhiteSpace(name))
                return Result.Failure<Tutorial>(Errors.TUTORIAL_NAME_INCORECT);

            if (basePath == null)
                return Result.Failure<Tutorial>(Errors.INVALID_DIRECTORY);

            if ((folders?.Count ?? 0) == 0)
                return Result.Failure<Tutorial>(Errors.NO_FOLDERS);

            if (dateTimeNow == DateTime.MinValue)
                return Result.Failure<Tutorial>(Errors.INVALID_DATE);

            return new Tutorial
            {
                Name = name.Trim(),
                BasePath = basePath,
                Folders = folders,
                AddDate = dateTimeNow,
                ModifiedTime = dateTimeNow,
            };
        }

        public void SetNewCategories(IEnumerable<Category> categories)
        {
            Guard.Against.Null(categories, nameof(categories));
            this._categories.Clear();
            this._categories.AddRange(categories.Distinct());
        }

        public IEnumerable<Folder> GetOrderedFolders()
            => this.Folders.OrderBy(o => o.Order);

        public IEnumerable<Episode> GetOrderedEpisode()
            => GetOrderedFolders().SelectMany(folder => folder.Episodes.OrderBy(o => o.Order));

        public Maybe<Episode> GetStartupEpisodeToPlay()
        {
            var firstUnwatchedEpisode = GetOrderedEpisode().FirstOrDefault(episode => !episode.IsWatched());

            if (firstUnwatchedEpisode is not null)
                return firstUnwatchedEpisode;

            return this.Folders.SelectMany(folder => folder.Episodes)
                               .FirstOrDefault();
        }

        public Maybe<Episode> GetNextEpisodeToPlay(Episode current)
        {
            Guard.Against.Null(current, nameof(current));
            return GetOrderedEpisode()
                        .SkipWhile(s => s != current)
                        .Skip(1)
                        .FirstOrDefault();
        }

        public Maybe<Episode> GetPrevEpisodeToPlay(Episode current)
        {
            Guard.Against.Null(current, nameof(current));
            return GetOrderedEpisode()
                        .TakeWhile(s => s != current)
                        .LastOrDefault();
        }
    }
}
