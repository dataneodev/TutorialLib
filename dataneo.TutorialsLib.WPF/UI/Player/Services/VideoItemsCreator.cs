using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class VideoItemsCreator
    {
        public async Task<Result<IReadOnlyList<object>>> GetAsync(int tutorialId)
        {
            Guard.Against.NegativeOrZero(tutorialId, nameof(tutorialId));
            var tutorialResult = await GetTutorialFromDBAsync(tutorialId);
            if (tutorialResult.IsFailure)
                return tutorialResult.ConvertFailure<IReadOnlyList<object>>();

            if (tutorialResult.Value.HasNoValue)
                return tutorialResult.Value
                            .ToResult($"Nie znaleziono tutorialu o id {tutorialId}")
                            .ConvertFailure<IReadOnlyList<object>>();

            return GetProcessedTutorial(tutorialResult.Value.Value)
                    .ToArray();
        }

        private static Task<Result<Maybe<Tutorial>>> GetTutorialFromDBAsync(int tutorialId)
        {
            using var repo = new TutorialRespositoryAsync();
            return Result.Try(async () => await repo.GetByIdAsync(tutorialId), error => error?.Message)
                         .Map(m => Maybe<Tutorial>.From(m));
        }

        private static IEnumerable<object> GetProcessedTutorial(Tutorial tutorial)
        {
            short folderPosition = 0;
            foreach (var folder in tutorial.Folders)
            {
                yield return GetFolderItem(folder, ref folderPosition);
                foreach (var episode in GetVideoItems(folder.Episodes))
                {
                    yield return episode;
                }
            }
        }

        private static FolderItem GetFolderItem(Folder folder, ref short folderPosition)
        {
            var watchStatus = folder.Episodes.Aggregate(
                                    VideoWatchStatus.Watched,
                                    SelectVideoWatchStatusForAllFolder);

            var folderPlayedTime = System.TimeSpan.FromSeconds(
                                    folder.Episodes.Sum(s => s.File.PlayTime.TotalSeconds));

            return new FolderItem
            {
                Position = ++folderPosition,
                Name = folder.Name,
                FolderPlayTime = folderPlayedTime,
                WatchStatus = watchStatus,
            };
        }

        private static IEnumerable<VideoItem> GetVideoItems(IReadOnlyList<Episode> episodes)
        {
            if (episodes.Count == 0)
                yield break;

            for (int i = 0; i < episodes.Count; i++)
            {
                yield return GetVideoItem(
                    episodes[i],
                    GetVideoItemLocationType(i, episodes.Count));
            }
        }

        private static VideoItemLocationType GetVideoItemLocationType(int i, int count)
        {
            if (count == 1)
                return VideoItemLocationType.InnerElement;

            if (i == 0)
                return VideoItemLocationType.FirstElement;

            if (i == count - 1)
                return VideoItemLocationType.LastElement;

            return VideoItemLocationType.InnerElement;
        }

        private static VideoItem GetVideoItem(Episode episode, VideoItemLocationType videoItemLocationType)
            => new VideoItem
            {
                EpisodePlayTime = episode.PlayedTime,
                Name = episode.Name,
                WatchStatus = episode.Status,
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
    }
}
