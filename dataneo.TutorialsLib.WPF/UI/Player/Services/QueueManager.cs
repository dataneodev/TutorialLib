using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using System;

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

        public void UserRequestEpisodePlay(int idEpisode)
        {
            if (this._currentPlayedEpisode is EpisodeData episodeData &&
                    episodeData.EpisodeD.Id != idEpisode)
            {
                this._currentPlayedEpisode.VideoItemD.WatchStatus = this._currentPlayedEpisode.EpisodeD.Status;
                SaveEpisodeToDBAsync(episodeData.EpisodeD);
            }

            EpisodePlay(idEpisode);
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
                    episode.Value.EpisodeD.GetPosition()));
        }

        public void SetPlayedEpisodePosition(int position)
        {
            var oldState = this._currentPlayedEpisode.EpisodeD.Status;
            this._currentPlayedEpisode.EpisodeD.SetPlayedTime(
                this._currentPlayedEpisode.EpisodeD.GetPlayedTime(position));

            var newState = this._currentPlayedEpisode.EpisodeD.Status;
            if (oldState != newState)
            {
                this._currentPlayedEpisode.VideoItemD.WatchStatus = newState;
                UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);
                SaveEpisodeToDBAsync(this._currentPlayedEpisode.EpisodeD);
            }
        }

        private static void SetAsPlaying(EpisodeData episode)
        {
            episode.VideoItemD.WatchStatus = VideoWatchStatus.InProgress;
        }

        private static void SetAsWatched(EpisodeData episode)
        {
            episode.VideoItemD.WatchStatus = VideoWatchStatus.Watched;
            episode.EpisodeD.SetAsWatched();
            SaveEpisodeToDBAsync(episode.EpisodeD);
        }

        private void UpdateFolderPlayedStatus(FolderItem folderItem)
        {
            //if (this._videoItemsCreatorResult.FoldersProcessed.All(a => a.Value.WatchStatus == VideoWatchStatus.NotWatched))

        }

        private static void SaveEpisodeToDBAsync(Episode episode)
        {

        }

        private Maybe<EpisodeData> FindEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetEpisodeData(idEpisode);

        private Maybe<int> GetNextEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetNextEpisodeId(idEpisode);
    }
}
