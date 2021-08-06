using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Entities;

namespace dataneo.TutorialsLibs.WPF.UI.Player.Services
{
    internal sealed class EpisodeData
    {
        public Tutorial TutorialD { get; }
        public Folder FolderD { get; }
        public Episode EpisodeD { get; }
        public VideoItem VideoItemD { get; }
        public FolderItem FolderItemD { get; }

        public EpisodeData(
                    Tutorial tutorialD,
                    Folder folderD,
                    Episode episodeD,
                    VideoItem videoItemD,
                    FolderItem folderItemD)
        {
            TutorialD = Guard.Against.Null(tutorialD, nameof(tutorialD));
            FolderD = Guard.Against.Null(folderD, nameof(folderD));
            EpisodeD = Guard.Against.Null(episodeD, nameof(episodeD));
            VideoItemD = Guard.Against.Null(videoItemD, nameof(videoItemD));
            FolderItemD = Guard.Against.Null(folderItemD, nameof(folderItemD));
        }

        public string GetEpisodePath()
            => this.EpisodeD?.GetFilePath(
                this.TutorialD,
                this.FolderD);
    }
}
