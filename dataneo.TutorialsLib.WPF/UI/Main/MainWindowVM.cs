using dataneo.TutorialLibs.Domain.DTO;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    internal sealed class MainWindowVM : BaseViewModel
    {
        public Action<bool> SetWindowVisibility;

        public ObservableCollection<TutorialHeaderDto> Tutorials = new ObservableCollection<TutorialHeaderDto>();

        public ICommand RatingChangedCommand { get; }
        public ICommand PlayTutorialCommand { get; }
        public ICommand AddTutorialCommand { get; }
        public ICommand SearchForUpdateCommand { get; }
        public ICommand SearchForNewTutorialsCommand { get; }

        public MainWindowVM()
        {
            this.RatingChangedCommand = new Command<ValueTuple<Guid, RatingStars>>(RatingChangedCommandImpl);
            this.PlayTutorialCommand = new Command<Guid>(PlayTutorialCommandImpl);
            this.AddTutorialCommand = new Command(AddTutorialCommandImpl);
        }

        private void AddTutorialCommandImpl()
        {

        }

        private void PlayTutorialCommandImpl(Guid tutorialId)
        {
            this.SetWindowVisibility?.Invoke(false);
            var playerWindow = new PlayerWindow(tutorialId, () => ClosePlayerWindow(tutorialId));
            playerWindow.Load();
        }

        private void RatingChangedCommandImpl(ValueTuple<Guid, RatingStars> tutorialIdAndRating)
        {

        }

        private void ClosePlayerWindow(Guid playedTutorialId)
        {
            this.SetWindowVisibility?.Invoke(true);
        }

        public async Task LoadAtStartupAsync()
        {
            using var repo = new TutorialRespositoryAsync();
            var tutorialHeaders = await repo.GetTutorialHeadersDtoByIdAsync();

            this.Tutorials.Clear();
            foreach (var tutorialHeader in tutorialHeaders)
            {
                this.Tutorials.Add(tutorialHeader);
            }
        }

        private IEnumerable<TutorialHeaderDto> GetFakeTutorials()
        {
            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(250),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
                LastPlayedDate = DateTime.Now,
                Rating = RatingStars.NotRated
            };

            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(170),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
                LastPlayedDate = DateTime.Now,
                Rating = RatingStars.FiveStars
            };

            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(300),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
                LastPlayedDate = DateTime.Now,
                Rating = RatingStars.ThreeStart
            };

            yield return new TutorialHeaderDto
            {
                DateAdd = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "Adding Search Abilities to Your Apps with Azure Search",
                PlayedEpisodes = 15,
                TimePlayed = TimeSpan.FromMinutes(340),
                TotalEpisodes = 125,
                TotalSizeMB = 632.3f,
                TotalTime = TimeSpan.FromMinutes(362),
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
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
                LastPlayedDate = DateTime.Now,
            };
        }
    }
}
