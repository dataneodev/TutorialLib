using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Domain.Translation;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public DirectoryPath BasePath { get; private set; }
        public RatingStars Rating { get; private set; }
        public IReadOnlyList<Folder> Folders { get; private set; }
        public DateTime AddDate { get; private set; }
        public DateTime ModifiedTime { get; private set; }

        public void SetRating(RatingStars ratingStars)
            => this.Rating = ratingStars;

        private Tutorial() { }

        public static Result<Tutorial> Create(
                        Guid id,
                        string name,
                        DirectoryPath basePath,
                        IReadOnlyList<Folder> folders,
                        DateTime dateTimeNow)
        {
            if (id == Guid.Empty)
                return Result.Failure<Tutorial>(Errors.EMPTY_GUID);

            if (String.IsNullOrWhiteSpace(name))
                return Result.Failure<Tutorial>(Errors.TUTORIAL_NAME_INCORECT);

            if (basePath == null)
                return Result.Failure<Tutorial>(Errors.INVALID_DIRECTORY);

            if ((folders?.Count ?? 0) == 0)
                return Result.Failure<Tutorial>(Errors.NO_FOLDERS);

            return new Tutorial
            {
                Id = id,
                Name = name,
                BasePath = basePath,
                Folders = folders,
                AddDate = dateTimeNow,
                ModifiedTime = dateTimeNow,
            };
        }
    }
}
