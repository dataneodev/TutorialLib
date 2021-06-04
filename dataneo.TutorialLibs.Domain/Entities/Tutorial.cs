using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Enums;
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
    }
}
