using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialsLibs.WPF.UI
{
    public class VideoItem : INotifyPropertyChanged
    {
        public int EpisodeId { get; init; }
        public string Name { get; init; }
        public VideoItemLocationType LocationOnList { get; init; }

        private VideoWatchStatus watchStatus;
        public VideoWatchStatus WatchStatus
        {
            get { return watchStatus; }
            set { watchStatus = value; Notify(); }
        }

        public TimeSpan EpisodePlayTime { get; init; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
