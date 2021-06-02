using dataneo.SharedKernel;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Entities
{
    public sealed class Tutorial : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public DirectoryPath BasePath { get; set; }
        public RatingStars Rating { get; private set; }
        public IReadOnlyList<Folder> Folders { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedTime { get; set; }

        public void SetRating(RatingStars ratingStars)
            => this.Rating = ratingStars;
    }
}
