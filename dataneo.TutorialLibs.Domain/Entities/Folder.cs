using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Folder : BaseEntity
    {
        private const int MinFolderName = 1;
        public Guid ParentTutorialId { get; private set; }
        public short Order { get; private set; }
        public string Name { get; private set; }
        public string FolderPath { get; private set; }
        public IReadOnlyList<Episode> Episodes { get; private set; }
        public bool IsRootFolder => string.IsNullOrWhiteSpace(FolderPath);

        private Folder() { }

        public static Result<Folder> Create(Guid id,
                                            Guid parentTutorialId,
                                            string folderPath,
                                            IReadOnlyList<Episode> episodes)
        {
            if (parentTutorialId == Guid.Empty)
                return Result.Failure<Folder>(Errors.EMPTY_PARENT_TUTORIAL_ID);

            if (id == Guid.Empty)
                return Result.Failure<Folder>(Errors.EMPTY_GUID);

            if ((episodes?.Count ?? 0) < 1)
                return Result.Failure<Folder>(Errors.NO_EPISODE);

            var folderPathTrimmed = folderPath?.Trim();

            if (String.IsNullOrWhiteSpace(folderPathTrimmed))
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            if (folderPathTrimmed.Length < MinFolderName)
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            var invalidChars = Path.GetInvalidPathChars();
            if (invalidChars.Any(c => folderPath.Contains(c)))
                return Result.Failure<Folder>(Errors.INVALID_DIRECTORY);

            return new Folder
            {
                Id = id,
                ParentTutorialId = parentTutorialId,
                FolderPath = folderPath,
                Name = folderPathTrimmed,
                Episodes = episodes,
            };
        }

        public void SetOrder(short order)
        {
            if (order < 0)
                throw new ArgumentException(nameof(order));
            Order = order;
        }
    }
}
