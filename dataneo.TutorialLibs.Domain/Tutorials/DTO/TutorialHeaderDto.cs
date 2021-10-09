using dataneo.TutorialLibs.Domain.Categories;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialLibs.Domain.Tutorials
{
    public record TutorialHeaderDto(
            int Id,
            string Name,
            TimeSpan TotalTime,
            TimeSpan TimePlayed,
            short PlayedEpisodes,
            short TotalEpisodes,
            DateTime LastPlayedDate,
            DateTime DateAdd,
            RatingStars Rating,
            float TotalSizeMB,
            IReadOnlyList<Category> Categories);
}
