using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreatorResult
    {
        public Tutorial Context { get; }
        public IReadOnlyList<KeyValuePair<Folder, FolderItem>> Folders { get; }
        public IReadOnlyList<KeyValuePair<Episode, VideoItem>> Episodes { get; }
        public IReadOnlyList<object> AllItems { get; }

        public VideoItemsCreatorResult(
                    Tutorial tutorial,
                    IReadOnlyList<KeyValuePair<Folder, FolderItem>> folders,
                    IReadOnlyList<KeyValuePair<Episode, VideoItem>> episodes,
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

            var episode = this.Episodes.FirstOrDefault(f => f.Key.Id == idEpisode);
            if (episode.Key is null)
                return Maybe<EpisodeData>.None;

            var folder = this.Folders.FirstOrDefault(f => f.Key.Episodes.Any(e => e == episode.Key));
            if (folder.Key is null)
                return Maybe<EpisodeData>.None;

            return new EpisodeData(
                this.Context,
                    folderD: folder.Key,
                    episodeD: episode.Key,
                    videoItemD: episode.Value,
                    folderItemD: folder.Value);
        }

        public Maybe<int> GetNextEpisodeId(int currentEpsiodeId)
        {
            for (int i = 0; i < Episodes.Count; i++)
            {
                if (Episodes[i].Key.Id == currentEpsiodeId && i < Episodes.Count - 1)
                    return Episodes[++i].Key.Id;
            }
            return Maybe<int>.None;
        }
    }
}
