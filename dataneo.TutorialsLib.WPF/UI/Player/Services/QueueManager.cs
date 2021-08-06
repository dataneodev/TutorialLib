using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using dataneo.TutorialLibs.Persistence.EF.SQLite.Respositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialsLibs.WPF.UI.Player.Services
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

            var nextEpisode = GetNextEpisode(this._currentPlayedEpisode.EpisodeD.Id);
            if (nextEpisode.HasValue)
                EpisodePlay(nextEpisode.Value);
        }

        public async void UserRequestEpisodePlay(int idEpisode)
        {
            if (this._currentPlayedEpisode is EpisodeData episodeData &&
                    episodeData.EpisodeD.Id != idEpisode)
            {
                this._currentPlayedEpisode.VideoItemD.WatchStatus = this._currentPlayedEpisode.EpisodeD.Status;
                UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
                await SaveEpisodeToDBAsync(episodeData.EpisodeD);
            }

            EpisodePlay(idEpisode);
        }

        public async Task EndWorkAsync()
        {
            if (this._currentPlayedEpisode.EpisodeD is null)
                return;

            await SaveEpisodeToDBAsync(this._currentPlayedEpisode.EpisodeD)
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
            var epsiodePlayPosition = episode.Value.EpisodeD.Status != VideoWatchStatus.Watched ?
                                        episode.Value.EpisodeD.GetPosition() : 0;
            this.BeginPlayFile?.Invoke(
                new PlayFileParameter(
                    episodePath,
                    episode.Value.EpisodeD.Name,
                    epsiodePlayPosition));
        }

        public async void SetPlayedEpisodePosition(int position)
        {
            var oldState = this._currentPlayedEpisode.EpisodeD.Status;
            var newPlayedTime = this._currentPlayedEpisode.EpisodeD.GetPlayedTime(position);

            if (Episode.GetWatchStatus(newPlayedTime, this._currentPlayedEpisode.EpisodeD.File.PlayTime) == VideoWatchStatus.NotWatched)
                return;

            this._currentPlayedEpisode.EpisodeD.SetPlayedTime(newPlayedTime);

            var newState = this._currentPlayedEpisode.EpisodeD.Status;
            if (oldState == newState)
                return;

            this._currentPlayedEpisode.VideoItemD.WatchStatus = newState;
            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
        }

        private static void SetAsPlaying(EpisodeData episode)
        {
            episode.VideoItemD.WatchStatus = VideoWatchStatus.InProgress;
        }

        private static async void SetAsWatched(EpisodeData episode)
        {
            episode.VideoItemD.WatchStatus = VideoWatchStatus.Watched;
            episode.EpisodeD.SetAsWatched();
            await SaveEpisodeToDBAsync(episode.EpisodeD)
                    .ConfigureAwait(false);
        }

        private void UpdateFolderPlayedStatus(FolderItem folderItem)
        {
            Maybe<Folder> folder = this._currentPlayedEpisode.TutorialD.Folders.FirstOrDefault(f => f.Id == folderItem.FolderId);
            if (folder.HasNoValue)
                return;
            var stateForFolder = VideoItemsCreatorEngine.GetFolderStatus(folder.Value.Episodes);
            folderItem.WatchStatus = stateForFolder;
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
