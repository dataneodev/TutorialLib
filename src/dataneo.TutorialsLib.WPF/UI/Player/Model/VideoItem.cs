using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Tutorials;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialLibs.WPF.UI
{
    public class VideoItem : INotifyPropertyChanged
    {
        private readonly Episode _episode;

        public VideoItemLocationType LocationOnList { get; init; }

        public Episode Episode => this._episode;
        public int EpisodeId => _episode.Id;
        public string Name => _episode.Name;
        public TimeSpan EpisodePlayTime => this._episode.File.PlayTime;
        public VideoWatchStatus WatchStatus => this._episode.Status;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public VideoItem(Episode episode)
        {
            this._episode = Guard.Against.Null(episode, nameof(episode));
        }

        public void SetPlayedTime(TimeSpan playedTime)
        {
            this.Episode.SetPlayedTime(playedTime, DateTime.Now);
            Notify(nameof(WatchStatus));
        }

        public void SetAsWatched()
        {
            this.Episode.SetAsWatched();
            Notify(nameof(WatchStatus));
        }
    }
}
