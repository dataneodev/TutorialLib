using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    internal sealed class QueueManager
    {
        private readonly VideoItemsCreatorResult _videoItemsCreatorResult;
        private EpisodeData _currentPlayedEpisode;

        public event Action<PlayFileParameter> BeginPlayFile;

        public QueueManager(VideoItemsCreatorResult videoItemsCreatorResult)
        {
            this._videoItemsCreatorResult = Guard.Against.Null(
                                                videoItemsCreatorResult,
                                                nameof(videoItemsCreatorResult));
        }

        public void StartupPlay()
        {
            var firstUnwatchedEpisode = this._videoItemsCreatorResult.Episodes
                                            .FirstOrDefault(f => f.Key.Status != VideoWatchStatus.Watched);
            if (firstUnwatchedEpisode.Key is null)
                return;
            EpisodePlay(firstUnwatchedEpisode.Key.Id);
        }

        public void CurrentPlayedEpisodeHasEnded()
        {
            Guard.Against.Null(
                this._currentPlayedEpisode,
                nameof(this._currentPlayedEpisode));

            SetAsWatched(this._currentPlayedEpisode);
            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            var nextEpisode = GetNextEpisode(this._currentPlayedEpisode.VideoItemD.EpisodeId);
            if (nextEpisode.HasValue)
                EpisodePlay(nextEpisode.Value);
        }

        public async void UserRequestEpisodePlay(int idEpisode)
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
            if (this._currentPlayedEpisode.VideoItemD?.Episode is null)
                return;

            await SaveEpisodeToDBAsync(this._currentPlayedEpisode.VideoItemD.Episode)
                    .ConfigureAwait(false);
        }

        private void EpisodePlay(int idEpisode)
        {
            var episode = FindEpisode(idEpisode);
            if (episode.HasNoValue)
                throw new InvalidOperationException();

            this._currentPlayedEpisode = episode.Value;

            SetAsPlaying(episode.Value);
            UpdateFolderPlayedStatus(episode.Value.FolderItemD);

            var episodePath = episode.Value.GetEpisodePath();
            var epsiodePlayPosition = episode.Value.VideoItemD.Episode.Status != VideoWatchStatus.Watched ?
                                        episode.Value.VideoItemD.Episode.GetPosition() : 0;
            this.BeginPlayFile?.Invoke(
                new PlayFileParameter(
                    episodePath,
                    episode.Value.TutorialD.Name,
                    episode.Value.FolderItemD.Name,
                    episode.Value.VideoItemD.Name,
                    episode.Value.VideoItemD.Episode.File.PlayTime,
                    epsiodePlayPosition));
        }

        public async void SetPlayedEpisodePosition(int position)
        {
            var oldState = this._currentPlayedEpisode.VideoItemD.Episode.Status;
            var newPlayedTime = this._currentPlayedEpisode.VideoItemD.Episode.GetPlayedTime(position);
            this._currentPlayedEpisode.VideoItemD.SetPlayedTime(newPlayedTime);

            if (Episode.GetWatchStatus(
                    newPlayedTime,
                    this._currentPlayedEpisode.VideoItemD.Episode.File.PlayTime) == VideoWatchStatus.NotWatched)
                return;

            var newState = this._currentPlayedEpisode.VideoItemD.Episode.Status;
            if (oldState == newState)
                return;

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
        }

        private static void SetAsPlaying(EpisodeData episode)
        {
            //episode.VideoItemD.WatchStatus = VideoWatchStatus.InProgress;
        }

        private static async void SetAsWatched(EpisodeData episode)
        {
            episode.VideoItemD.Episode.SetAsWatched();
            await SaveEpisodeToDBAsync(episode.VideoItemD.Episode)
                    .ConfigureAwait(false);
        }

        private void UpdateFolderPlayedStatus(FolderItem folderItem)
        {
            folderItem.NotifyWatchStatus();
        }

        private static async Task SaveEpisodeToDBAsync(Episode episode)
        {
            using var repo = new TutorialRespositoryAsync();
            await repo.UpdateEpisodeAsync(episode);
        }

        private Maybe<EpisodeData> FindEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetEpisodeData(idEpisode);

        private Maybe<int> GetNextEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetNextEpisodeId(idEpisode);
    }
}
