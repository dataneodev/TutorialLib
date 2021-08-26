using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.DTO
{
    public sealed class TutorialHeaderDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public TimeSpan TotalTime { get; init; }
        public TimeSpan TimePlayed { get; init; }
        public short PlayedEpisodes { get; init; }
        public short TotalEpisodes { get; init; }
        public DateTime LastPlayedDate { get; init; }
        public DateTime DateAdd { get; init; }
        public RatingStars Rating { get; init; }
        public float TotalSizeMB { get; init; }
        public IReadOnlyList<Category> Categories { get; init; }
    }
}
