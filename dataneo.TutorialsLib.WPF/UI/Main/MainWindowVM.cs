using dataneo.TutorialLibs.Domain.DTO;
using System;
using System.Collections.Generic;

namespace dataneo.TutorialsLib.WPF.UI.Main
{
    internal sealed class MainWindowVM : BaseViewModel
    {
        public IEnumerable<TutorialHeaderDto> Tutorials => GetFakeTutorials();


        private IEnumerable<TutorialHeaderDto> GetFakeTutorials()
        {
            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(62),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
            };

            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(62),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
            };
        }

    }
}
