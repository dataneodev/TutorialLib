using System;

namespace dataneo.TutorialLibs.Domain.DTO
{
    public sealed class TutorialHeaderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan TimePlayed { get; set; }
        public short Episodes { get; set; }
        public short PlayedEpisodes { get; set; }
        public short TotalEpisodes { get; set; }
        public DateTime DateAdd { get; set; }
        public float TotalSizeMB { get; set; }
    }
}
