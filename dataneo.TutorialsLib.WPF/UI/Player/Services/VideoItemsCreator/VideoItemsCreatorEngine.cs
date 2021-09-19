using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreatorEngine
    {
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;

        public VideoItemsCreatorEngine(ITutorialRespositoryAsync tutorialRespositoryAsync)
        {
            this._tutorialRespositoryAsync = Guard.Against.Null(
                                                tutorialRespositoryAsync,
                                                nameof(tutorialRespositoryAsync));
        }

        public async Task<Result<VideoItemsCreatorResult>> LoadAndCreate(int idTutorial)
        {
            Guard.Against.NegativeOrZero(idTutorial, nameof(idTutorial));
            var tutorialResult = await GetTutorialFromDBAsync(
                                        tutorialId: idTutorial,
                                        tutorialRespositoryAsync: this._tutorialRespositoryAsync);
            if (tutorialResult.IsFailure)
                return tutorialResult.ConvertFailure<VideoItemsCreatorResult>();

            if (tutorialResult.Value.HasNoValue)
                return Result.Failure<VideoItemsCreatorResult>($"Nie znaleziono tutorialu o id {idTutorial}");

            return Create(tutorialResult.Value.Value);
        }

        public static VideoItemsCreatorResult Create(Tutorial tutorial)
        {
            Guard.Against.Null(tutorial, nameof(tutorial));

            var foldersProcessed = new List<FolderItem>(tutorial.Folders.Count);
            var episodesProcessed = new List<VideoItem>(tutorial.Folders.Sum(s => s.Episodes.Count));
            var allItems = new List<object>(foldersProcessed.Count + episodesProcessed.Count);

            short folderPosition = 0;
            foreach (var folder in tutorial.Folders.OrderBy(o => o.Order))
            {
                var folderProcessed = GetFolderItem(folder, ref folderPosition);

                foldersProcessed.Add(folderProcessed);
                allItems.Add(folderProcessed);

                foreach (var episodeProcess in GetVideoItems(folder.Episodes))
                {
                    episodesProcessed.Add(episodeProcess);
                    allItems.Add(episodeProcess);
                }
            }

            return new VideoItemsCreatorResult(
                            tutorial: tutorial,
                            folders: foldersProcessed,
                            episodes: episodesProcessed,
                            allItems: allItems);
        }

        public static VideoWatchStatus GetFolderStatus(IEnumerable<Episode> episodes)
        {
            Guard.Against.Null(episodes, nameof(episodes));
            return episodes.Aggregate(
                VideoWatchStatus.Watched,
                SelectVideoWatchStatusForAllFolder);
        }

        private static FolderItem GetFolderItem(Folder folder, ref short folderPosition)
        {
            var watchStatus = GetFolderStatus(folder.Episodes);
            var folderItem = new FolderItem(folder)
            {
                Position = ++folderPosition,
            };

            return folderItem;
        }

        private static IEnumerable<VideoItem> GetVideoItems(IReadOnlyList<Episode> episodes)
        {
            if (episodes.Count == 0)
                return Enumerable.Empty<VideoItem>();

            return episodes
                .OrderBy(o => o.Order)
                .Select((episode, index) => GetEpisodeVideoItem(episodes.Count, episode, index));
        }

        private static VideoItem GetEpisodeVideoItem(int episodeCount, Episode episode, int index)
            => GetVideoItem(episode, GetVideoItemLocationType(index, episodeCount));

        private static VideoItemLocationType GetVideoItemLocationType(int i, int count)
        {
            if (count == 1)
                return VideoItemLocationType.AloneElement;

            if (i == 0)
                return VideoItemLocationType.FirstElement;

            if (i == count - 1)
                return VideoItemLocationType.LastElement;

            return VideoItemLocationType.InnerElement;
        }

        private static VideoItem GetVideoItem(Episode episode, VideoItemLocationType videoItemLocationType)
            => new VideoItem(episode)
            {
                LocationOnList = videoItemLocationType,
            };

        private static VideoWatchStatus SelectVideoWatchStatusForAllFolder(
                            VideoWatchStatus aggregate,
                            Episode episode)
        {
            if (aggregate == VideoWatchStatus.InProgress)
                return aggregate;

            if (episode.Status != VideoWatchStatus.Watched)
                return episode.Status;

            return VideoWatchStatus.Watched;
        }

        private static Task<Result<Maybe<Tutorial>>> GetTutorialFromDBAsync(int tutorialId,
                                                        ITutorialRespositoryAsync tutorialRespositoryAsync)
            => Result.Try(async () => await tutorialRespositoryAsync.GetByIdAsync(tutorialId), error => error?.Message)
                     .Map(m => Maybe<Tutorial>.From(m));
    }
}
