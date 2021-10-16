using Ardalis.GuardClauses;
using System;

namespace dataneo.TutorialLibs.WPF.UI
{
    public sealed class PlayFileParameter
    {
        public PlayFileParameter(
                        string path,
                        string tutorialTitle,
                        string folderTitle,
                        string episodeTitle,
                        TimeSpan playTime,
                        int position,
                        object item)
        {
            Path = Guard.Against.NullOrWhiteSpace(path, nameof(path));
            TutorialTitle = Guard.Against.NullOrWhiteSpace(tutorialTitle, nameof(tutorialTitle));
            FolderTitle = Guard.Against.NullOrWhiteSpace(folderTitle, nameof(folderTitle));
            EpisodeTitle = Guard.Against.NullOrWhiteSpace(episodeTitle, nameof(episodeTitle));
            PlayTime = playTime;
            Position = position;
            Item = item;
        }

        public string Path { get; }
        public string TutorialTitle { get; }
        public string FolderTitle { get; }
        public string EpisodeTitle { get; }
        public TimeSpan PlayTime { get; }
        public int Position { get; }
        public object Item { get; }
    }
}
