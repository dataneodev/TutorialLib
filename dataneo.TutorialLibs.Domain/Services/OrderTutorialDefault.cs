using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace dataneo.TutorialLibs.Domain.Services
{
    internal sealed class OrderTutorialDefault
    {
        public void OrderFoldersAndEpisodesByName(Tutorial tutorial)
        {
            Guard.Against.Null(tutorial, nameof(tutorial));
            OrderFolders(tutorial.Folders);
        }

        private static void OrderFolders(IReadOnlyList<Folder> folders)
        {
            short orderFolder = 1;
            foreach (var folder in folders.OrderBy(o => o.IsRootFolder)
                                          .ThenBy(o => o.FolderPath))
            {
                folder.SetOrder(orderFolder++);
                OrderEpisodes(folder.Episodes);
            }
        }

        private static void OrderEpisodes(IReadOnlyList<Episode> episodes)
        {
            short orderEpisode = 1;
            foreach (var episode in episodes.OrderByDescending(o => o.Name))
            {
                episode.SetOrder(orderEpisode++);
            }
        }
    }
}
