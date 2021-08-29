﻿using Ardalis.GuardClauses;
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

            UpdateFolderPlayedStatus(episode.Value.FolderItemD);

            var episodePath = episode.Value.GetEpisodePath();
            int epsiodePlayPosition = GetEpisodePosition(episode.Value);

            this.BeginPlayFile?.Invoke(
                new PlayFileParameter(
                    episodePath,
                    episode.Value.TutorialD.Name,
                    episode.Value.FolderItemD.Name,
                    episode.Value.VideoItemD.Name,
                    episode.Value.VideoItemD.Episode.File.PlayTime,
                    epsiodePlayPosition,
                    episode.Value.VideoItemD));
        }

        private static int GetEpisodePosition(EpisodeData episode)
            => episode.VideoItemD.WatchStatus == VideoWatchStatus.InProgress ?
                    episode.VideoItemD.Episode.GetPosition() : 0;

        public async void SetPlayedEpisodePositionAsync(int position)
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
                return;

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            await SaveEpisodeToDBAsync(this._currentPlayedEpisode.VideoItemD.Episode)
                .ConfigureAwait(false);

            EpisodePlay(nextEpisode.Value);
        }

        public async void PlayPrevEpisodeAsync()
        {
            if (this._currentPlayedEpisode is null)
                return;

            var nextEpisode = GetPrevEpisode(this._currentPlayedEpisode.VideoItemD.EpisodeId);
            if (nextEpisode.HasNoValue)
                return;

            UpdateFolderPlayedStatus(this._currentPlayedEpisode.FolderItemD);

            await SaveEpisodeToDBAsync(this._currentPlayedEpisode.VideoItemD.Episode)
                .ConfigureAwait(false);

            EpisodePlay(nextEpisode.Value);
        }

        private static async void SetAsWatchedAsync(EpisodeData episode)
        {
            episode.VideoItemD.SetAsWatched();
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

        private Maybe<int> GetPrevEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetPrevEpisodeId(idEpisode);

        private Maybe<int> GetNextEpisode(int idEpisode)
            => this._videoItemsCreatorResult.GetNextEpisodeId(idEpisode);
    }
}
