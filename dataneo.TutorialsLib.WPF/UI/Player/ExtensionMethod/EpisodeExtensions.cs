using dataneo.TutorialLibs.Domain.Entities;
using System;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal static class EpisodeExtensions
    {
        public static int GetPosition(this Episode episode)
            => (int)Math.Ceiling(episode.PlayedTime.TotalSeconds / episode.File.PlayTime.TotalSeconds);

        public static TimeSpan GetPlayedTime(this Episode episode, int position)
            => TimeSpan.FromSeconds((position / 100f) * episode.File.PlayTime.TotalSeconds);
    }
}
