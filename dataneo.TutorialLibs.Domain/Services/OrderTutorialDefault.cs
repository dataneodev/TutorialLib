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

        private void OrderFolders(IReadOnlyList<Folder> folders)
        {
            short orderFolder = 1;
            foreach (var folder in folders.OrderBy(o => o.IsRootFolder)
                                          .ThenBy(o => o.FolderName))
            {
                folder.Order = orderFolder++;
                OrderEpisodes(folder.Episodes);
            }
        }

        private void OrderEpisodes(IReadOnlyList<Episode> episodes)
        {
            short orderEpisode = 1;
            foreach (var episode in episodes.OrderBy(o => o.Name))
            {
                episode.Order = orderEpisode++;
            }
        }
    }
}
