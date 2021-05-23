using CSharpFunctionalExtensions;
using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Translation;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Folder : BaseEntity
    {
        public Guid ParentTutorialId { get; private set; }
        public short Order { get; set; }
        public string Name { get; set; }
        public string FolderName { get; private set; }
        public IReadOnlyList<Episode> Episodes { get; set; }
        public bool IsRootFolder => string.IsNullOrWhiteSpace(FolderName);

        private Folder() { }

        public static Result<Folder> Create(Guid parentTutorialId, string folderName)
        {
            if (parentTutorialId == Guid.Empty)
                Result.Failure<Folder>(Errors.EMPTY_PARENT_TUTORIAL_ID);

            return new Folder
            {
                ParentTutorialId = parentTutorialId,
                FolderName = folderName,
            };
        }
    }
}
