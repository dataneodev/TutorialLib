using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreatorResult
    {
        public Tutorial ProcessedTutorial { get; }
        public IReadOnlyList<KeyValuePair<Folder, FolderItem>> FoldersProcessed { get; }
        public IReadOnlyList<KeyValuePair<Episode, VideoItem>> EpisodesProcessed { get; }
        public IReadOnlyList<object> AllItemsProcessed { get; }

        public VideoItemsCreatorResult(
                    Tutorial processedTutorial,
                    IReadOnlyList<KeyValuePair<Folder, FolderItem>> foldersProcessed,
                    IReadOnlyList<KeyValuePair<Episode, VideoItem>> episodesProcessed,
                    IReadOnlyList<object> allItemsProcessed)
        {
            ProcessedTutorial = Guard.Against.Null(processedTutorial, nameof(processedTutorial));
            FoldersProcessed = Guard.Against.Null(foldersProcessed, nameof(foldersProcessed));
            EpisodesProcessed = Guard.Against.Null(episodesProcessed, nameof(episodesProcessed));
            AllItemsProcessed = Guard.Against.Null(allItemsProcessed, nameof(allItemsProcessed));
        }

        public Maybe<EpisodeData> GetEpisodeData(int idEpisode)
        {
            Guard.Against.NegativeOrZero(idEpisode, nameof(idEpisode));

            var episode = this.EpisodesProcessed.FirstOrDefault(f => f.Key.Id == idEpisode);
            if (episode.Key is null)
                return Maybe<EpisodeData>.None;

            var folder = this.FoldersProcessed.FirstOrDefault(f => f.Key.Episodes.Any(e => e == episode.Key));
            if (folder.Key is null)
                return Maybe<EpisodeData>.None;

            return new EpisodeData(
                this.ProcessedTutorial,
                    folderD: folder.Key,
                    episodeD: episode.Key,
                    videoItemD: episode.Value,
                    folderItemD: folder.Value);
        }

        public Maybe<int> GetNextEpisodeId(int currentEpsiodeId)
        {
            for (int i = 0; i < EpisodesProcessed.Count; i++)
            {
                if (EpisodesProcessed[i].Key.Id == currentEpsiodeId && i < EpisodesProcessed.Count - 1)
                    return EpisodesProcessed[++i].Key.Id;
            }
            return Maybe<int>.None;
        }
    }
}
