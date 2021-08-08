using dataneo.TutorialLibs.Domain.Entities;
using System;

namespace dataneo.TutorialLibs.WPF.UI
{
    internal static class EpisodeExtensions
    {
        public static int GetPosition(this Episode episode)
            => (int)Math.Ceiling(100 * episode.PlayedTime.TotalSeconds / (float)episode.File.PlayTime.TotalSeconds);

        public static TimeSpan GetPlayedTime(this Episode episode, int position)
            => TimeSpan.FromSeconds((position / 100f) * episode.File.PlayTime.TotalSeconds);
    }
}
