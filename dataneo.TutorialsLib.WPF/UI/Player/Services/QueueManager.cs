using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using System;
using System.Linq;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
{
    internal sealed class QueueManager
    {
        private readonly VideoItemsCreatorResult _videoItemsCreatorResult;
        private int _currentPlayedEpisode;

        public event Action<string> BeginPlayFile;

        public QueueManager(VideoItemsCreatorResult videoItemsCreatorResult)
        {
            this._videoItemsCreatorResult = Guard.Against.Null(
                                                videoItemsCreatorResult,
                                                nameof(videoItemsCreatorResult));
        }

        public void StartupPlay()
        {


        }

        public void CurrentPlayedEpisodeHasEnded()
        {


        }

        public void UserRequestEpisodePlay(int idEpisode)
        {
            var episode = FindEpisode(idEpisode);
            if (episode.HasNoValue)
                throw new InvalidOperationException();

            var episodePath = episode.Value.GetEpisodePath();
            this.BeginPlayFile?.Invoke(episodePath);
        }

        public void SetEpisodeState(int idEpisode)
        {


        }



        private Maybe<EpisodeData> FindEpisode(int idEpisode)
        {
            var episode = this._videoItemsCreatorResult


            foreach (var folder in this._videoItemsCreatorResult.ProcessedTutorial.Folders)
            {
                var episode = folder.Episodes.FirstOrDefault(f => f.Id == idEpisode);
                if (episode == null)
                    continue;

                return new EpisodeData
                {
                    TutorialD = this._videoItemsCreatorResult.ProcessedTutorial,
                    FolderD = folder,
                    EpisodeD = episode,
                };
            }
            return Maybe<EpisodeData>.None;
        }

    }
}
