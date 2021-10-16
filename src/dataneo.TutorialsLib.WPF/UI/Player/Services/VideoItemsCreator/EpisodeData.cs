using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Tutorials;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    internal sealed class EpisodeData
    {
        public Tutorial TutorialD { get; }
        public VideoItem VideoItemD { get; }
        public FolderItem FolderItemD { get; }

        public Episode Episode => this.VideoItemD.Episode;
        public Folder Fodler => this.FolderItemD.Folder;

        public EpisodeData(
                    Tutorial tutorialD,
                    VideoItem videoItemD,
                    FolderItem folderItemD)
        {
            TutorialD = Guard.Against.Null(tutorialD, nameof(tutorialD));
            VideoItemD = Guard.Against.Null(videoItemD, nameof(videoItemD));
            FolderItemD = Guard.Against.Null(folderItemD, nameof(folderItemD));
        }

        public string GetEpisodePath()
            => this.VideoItemD.Episode.GetFilePath(
                this.TutorialD,
                this.FolderItemD.Folder);
    }
}
