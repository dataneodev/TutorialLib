using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreatorResult
    {
        public Tutorial Context { get; }
        public IReadOnlyList<FolderItem> Folders { get; }
        public IReadOnlyList<VideoItem> Episodes { get; }
        public IReadOnlyList<object> AllItems { get; }

        public VideoItemsCreatorResult(
                    Tutorial tutorial,
                    IReadOnlyList<FolderItem> folders,
                    IReadOnlyList<VideoItem> episodes,
                    IReadOnlyList<object> allItems)
        {
            Context = Guard.Against.Null(tutorial, nameof(tutorial));
            Folders = Guard.Against.Null(folders, nameof(folders));
            Episodes = Guard.Against.Null(episodes, nameof(episodes));
            AllItems = Guard.Against.Null(allItems, nameof(allItems));
        }

        public Maybe<EpisodeData> GetEpisodeData(int idEpisode)
        {
            Guard.Against.NegativeOrZero(idEpisode, nameof(idEpisode));

            var episode = this.Episodes.FirstOrDefault(f => f.EpisodeId == idEpisode);
            if (episode is null)
                return Maybe<EpisodeData>.None;

            var folder = this.Folders.FirstOrDefault(f => f.Folder.Episodes.Any(e => e == episode.Episode));
            if (folder is null)
                return Maybe<EpisodeData>.None;

            return new EpisodeData(
                tutorialD: this.Context,
                videoItemD: episode,
                folderItemD: folder);
        }

        public Maybe<int> GetNextEpisodeId(int currentEpsiodeId)
        {
            var id = this.Episodes.SkipWhile(s => s.EpisodeId != currentEpsiodeId)
                         .Skip(1)
                         .FirstOrDefault()?.EpisodeId;
            if (id.HasValue)
                return id.Value;

            return Maybe<int>.None;
        }

        public Maybe<int> GetPrevEpisodeId(int currentEpsiodeId)
        {
            var id = this.Episodes.TakeWhile(s => s.EpisodeId != currentEpsiodeId)
                         .LastOrDefault()?.EpisodeId;
            if (id.HasValue)
                return id.Value;

            return Maybe<int>.None;
        }
    }
}
