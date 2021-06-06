using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;

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
                                            string name,
                                            IReadOnlyList<Episode> episodes)
        {
            if (parentTutorialId == Guid.Empty)
                return Result.Failure<Folder>(Errors.EMPTY_PARENT_TUTORIAL_ID);

            if (id == Guid.Empty)
                return Result.Failure<Folder>(Errors.EMPTY_GUID);


            var nameTrimed = name.Trim();


            return new Folder
            {
                Id = id,
                ParentTutorialId = parentTutorialId,
                FolderPath = folderPath,
                Name = name,
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
