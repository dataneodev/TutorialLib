using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class QueueManager
    {
        private readonly int _tutorialId;
        private Tutorial loadedTutorial;
        private int _currentPlayedEpisode;

        public event Action<string> BeginPlayFile;

        public QueueManager(int tutorialId)
        {
            this._tutorialId = Guard.Against.NegativeOrZero(tutorialId, nameof(tutorialId));
        }

        public async Task<Result<IReadOnlyList<object>>> LoadVideoItemsAsync()
        {
            this.loadedTutorial = null;
            var tutorialResult = await GetTutorialFromDBAsync(this._tutorialId);
            if (tutorialResult.IsFailure)
                return tutorialResult.ConvertFailure<IReadOnlyList<object>>();

            if (tutorialResult.Value.HasNoValue)
                return tutorialResult.Value
                            .ToResult($"Nie znaleziono tutorialu o id {this._tutorialId}")
                            .ConvertFailure<IReadOnlyList<object>>();

            this.loadedTutorial = tutorialResult.Value.Value;

            return VideoItemsCreator.Create(tutorialResult.Value.Value);
        }

        public void StartupPlay()
        {
            GuardAgainsTutorialLoaded();


        }

        public void CurrentPlayedEpisodeHasEnded()
        {
            GuardAgainsTutorialLoaded();


        }

        public void UserRequestEpisodePlay(int idEpisode)
        {
            GuardAgainsTutorialLoaded();
            var episode = FindEpisode(idEpisode);
            if (episode.HasNoValue)
                throw new InvalidOperationException();

            var episodePath = episode.Value.GetEpisodePath();
            this.BeginPlayFile?.Invoke(episodePath);

        }

        public void SetEpisodeState(int idEpisode)
        {
            GuardAgainsTutorialLoaded();


        }

        private static Task<Result<Maybe<Tutorial>>> GetTutorialFromDBAsync(int tutorialId)
        {
            using var repo = new TutorialRespositoryAsync();
            return Result.Try(async () => await repo.GetByIdAsync(tutorialId), error => error?.Message)
                         .Map(m => Maybe<Tutorial>.From(m));
        }

        private void GuardAgainsTutorialLoaded()
        {
            if (this.loadedTutorial == null)
                throw new InvalidOperationException();
        }

        private struct EpisodeData
        {
            public Tutorial TutorialD;
            public Folder FolderD;
            public Episode EpisodeD;

            public string GetEpisodePath()
                => this.EpisodeD?.GetFilePath(
                    this.TutorialD,
                    this.FolderD);
        }

        private Maybe<EpisodeData> FindEpisode(int idEpisode)
        {
            foreach (var folder in this.loadedTutorial.Folders)
            {
                var episode = folder.Episodes.FirstOrDefault(f => f.Id == idEpisode);
                if (episode == null)
                    continue;

                return new EpisodeData
                {
                    TutorialD = this.loadedTutorial,
                    FolderD = folder,
                    EpisodeD = episode,
                };
            }
            return Maybe<EpisodeData>.None;
        }

    }
}
