using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Tutorials;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    internal sealed class QueueManager
    {
        private readonly ITutorialRespositoryAsync _tutorialRespositoryAsync;
        private readonly VideoItemsCreatorResult _videoItemsCreatorResult;
        private EpisodeData _currentPlayedEpisode;

        public event Action<PlayFileParameter> BeginPlayFile;

        public QueueManager(ITutorialRespositoryAsync tutorialRespositoryAsync, VideoItemsCreatorResult videoItemsCreatorResult)
        {
            this._videoItemsCreatorResult = Guard.Against.Null(
                                                videoItemsCreatorResult,
                                                nameof(videoItemsCreatorResult));
            this._tutorialRespositoryAsync = Guard.Against.Null(
                                                tutorialRespositoryAsync,
                                                nameof(tutorialRespositoryAsync));
        }

        public void StartupPlay()
        {
            var firstUnwatchedEpisode = this._videoItemsCreatorResult.Episodes
                                            .FirstOrDefault(f => f.WatchStatus != VideoWatchStatus.Watched);
            if (firstUnwatchedEpisode is null)
                return;
            EpisodePlay(firstUnwatchedEpisode.EpisodeId);
        }

        public void CurrentPlayedEpisodeHasEnded()
        {
            Guard.Against.Null(
                this._currentPlayedEpisode,
                nameof(this._currentPlayedEpisode));

            SetAsWatchedAsync(this._currentPlayedEpisode);
            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            var nextEpisode = GetNextEpisode(this._currentPlayedEpisode.VideoItemD.EpisodeId);
            if (nextEpisode.HasValue)
                EpisodePlay(nextEpisode.GetValueOrThrow());
        }

        public void UserRequestEpisodePlay(int idEpisode)
        {
            if (this._currentPlayedEpisode is EpisodeData episodeData &&
                episodeData.VideoItemD.EpisodeId != idEpisode)
            {
                UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
            }

            EpisodePlay(idEpisode);
        }

        public async Task EndWorkAsync()
        {
            if (this._currentPlayedEpisode?.Episode is null)
                return;

            await SaveTutorialToDBAsync(this._currentPlayedEpisode.TutorialD)
                    .ConfigureAwait(false);
        }

        private async Task StopPlayingAsync()
        {
            await EndWorkAsync();
            this.BeginPlayFile?.Invoke(null);
        }

        private void EpisodePlay(int idEpisode)
        {
            var episode = FindEpisode(idEpisode);
            if (episode.HasNoValue)
                throw new InvalidOperationException();

            this._currentPlayedEpisode = episode.GetValueOrThrow();

            UpdateFolderPlayedStatus(episode.GetValueOrThrow().FolderItemD);

            var episodePath = episode.GetValueOrThrow().GetEpisodePath();
            int epsiodePlayPosition = GetEpisodePosition(episode.GetValueOrThrow());

            this.BeginPlayFile?.Invoke(
                new PlayFileParameter(
                    episodePath,
                    episode.GetValueOrThrow().TutorialD.Name,
                    episode.GetValueOrThrow().FolderItemD.Name,
                    episode.GetValueOrThrow().VideoItemD.Name,
                    episode.GetValueOrThrow().VideoItemD.Episode.File.PlayTime,
                    epsiodePlayPosition,
                    episode.GetValueOrThrow().VideoItemD));
        }

        private static int GetEpisodePosition(EpisodeData episode)
            => episode.VideoItemD.WatchStatus == VideoWatchStatus.InProgress ?
                    episode.VideoItemD.Episode.GetPosition() : 0;

        public void SetPlayedEpisodePosition(int position)
        {
            var oldState = this._currentPlayedEpisode.VideoItemD.WatchStatus;
            var newPlayedTime = this._currentPlayedEpisode.VideoItemD.Episode.GetPlayedTime(position);
            this._currentPlayedEpisode.VideoItemD.SetPlayedTime(newPlayedTime);

            var newState = this._currentPlayedEpisode.VideoItemD.WatchStatus;
            if (oldState == newState || newState == VideoWatchStatus.NotWatched)
                return;

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
        }

        public async void PlayNextEpisodeAsync()
        {
            if (this._currentPlayedEpisode is null)
                return;

            var nextEpisode = GetNextEpisode(this._currentPlayedEpisode.VideoItemD.EpisodeId);
            if (nextEpisode.HasNoValue)
            {
                await StopPlayingAsync();
                return;
            }

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            await SaveTutorialToDBAsync(this._currentPlayedEpisode.TutorialD);

            EpisodePlay(nextEpisode.GetValueOrThrow());
        }

        public async void PlayPrevEpisodeAsync()
        {
            if (this._currentPlayedEpisode is null)
                return;

            var nextEpisode = GetPrevEpisode(this._currentPlayedEpisode.VideoItemD.EpisodeId);
            if (nextEpisode.HasNoValue)
            {
                await StopPlayingAsync();
                return;
            }

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            await SaveTutorialToDBAsync(this._currentPlayedEpisode.TutorialD);

            EpisodePlay(nextEpisode.GetValueOrThrow());
        }

        private async void SetAsWatchedAsync(EpisodeData episode)
        {
            episode.VideoItemD.SetAsWatched();
            await SaveTutorialToDBAsync(episode.TutorialD)
                    .ConfigureAwait(false);
        }

        private void UpdateFolderPlayedStatus(FolderItem folderItem)
        {
            folderItem.NotifyWatchStatus();
        }

        private async Task SaveTutorialToDBAsync(Tutorial tutorial)
        {
            await this._tutorialRespositoryAsync.UpdateAsync(tutorial)
                            .ConfigureAwait(false);
        }

        private Maybe<EpisodeData> FindEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetEpisodeData(idEpisode);

        private Maybe<int> GetPrevEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetPrevEpisodeId(idEpisode);

        private Maybe<int> GetNextEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetNextEpisodeId(idEpisode);
    }
}
