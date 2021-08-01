using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using System.Collections.Generic;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreatorResult
    {
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

        public Tutorial ProcessedTutorial { get; }
        public IReadOnlyList<KeyValuePair<Folder, FolderItem>> FoldersProcessed { get; }
        public IReadOnlyList<KeyValuePair<Episode, VideoItem>> EpisodesProcessed { get; }
        public IReadOnlyList<object> AllItemsProcessed { get; }

        public Maybe<EpisodeData> GetEpisodeData(int idEpisode)
        {
            Guard.Against.NegativeOrZero(idEpisode, nameof(idEpisode));

            return Maybe<EpisodeData>.None;
        }
    }
}
