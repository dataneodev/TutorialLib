using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialsLib.WPF.UI
{
    public class FolderItem : INotifyPropertyChanged
    {
        public int FolderId { get; init; }
        public short Position { get; set; }
        public string Name { get; init; }
        public TimeSpan FolderPlayTime { get; init; }

        private VideoWatchStatus watchStatus;
        public VideoWatchStatus WatchStatus
        {
            get { return watchStatus; }
            set { watchStatus = value; Notify(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
