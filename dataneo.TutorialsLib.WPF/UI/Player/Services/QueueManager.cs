using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dataneo.TutorialsLib.WPF.UI.Player.Services
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

        public void NextEpisode()
        {


        }

        public void PrevExpisode()
        {



        }

        public async Task EndWorkAsync()
        {
            if (this._currentPlayedEpisode.EpisodeD is not null)
            {
                await SaveEpisodeToDBAsync(this._currentPlayedEpisode.EpisodeD)
                        .ConfigureAwait(false);
            }
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
            this.BeginPlayFile?.Invoke(
                new PlayFileParameter(
                    episodePath,
                    episode.Value.EpisodeD.Name,
                    episode.Value.EpisodeD.GetPosition()));
        }

        public async void SetPlayedEpisodePosition(int position)
        {
            var oldState = this._currentPlayedEpisode.EpisodeD.Status;
            this._currentPlayedEpisode.EpisodeD.SetPlayedTime(
                this._currentPlayedEpisode.EpisodeD.GetPlayedTime(position));

            var newState = this._currentPlayedEpisode.EpisodeD.Status;
            if (oldState != newState)
            {
                this._currentPlayedEpisode.VideoItemD.WatchStatus = newState;
                UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
                await SaveEpisodeToDBAsync(this._currentPlayedEpisode.EpisodeD)
                        .ConfigureAwait(false);
            }
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

            //  if (folder.Value.Episodes.All(a => a.Status =))
            //if (this._videoItemsCreatorResult.FoldersProcessed.All(a => a.Value.WatchStatus == VideoWatchStatus.NotWatched))

        }

        private static async Task SaveEpisodeToDBAsync(Episode episode)
        {

        }

        private Maybe<EpisodeData> FindEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetEpisodeData(idEpisode);

        private Maybe<int> GetNextEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetNextEpisodeId(idEpisode);
    }
}
