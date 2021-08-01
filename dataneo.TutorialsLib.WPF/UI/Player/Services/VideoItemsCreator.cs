using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal static class VideoItemsCreator
    {
        public static Result<IReadOnlyList<object>> Create(Tutorial tutorial)
            => GetProcessedTutorial(tutorial).ToArray();

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
                FolderId = folder.Id,
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
                EpisodeId = episode.Id,
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
