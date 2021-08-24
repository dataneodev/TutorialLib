using Ardalis.GuardClauses;
using dataneo.TutorialLibs.Domain.Entities;
using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialLibs.WPF.UI
{
    public class FolderItem : INotifyPropertyChanged
    {
        private readonly Folder _folder;
        public Folder Folder => this._folder;
        public int FolderId => this._folder.Id;
        public string Name => this._folder.Name;
        public TimeSpan FolderPlayTime => this._folder.GetFolderPlayedTime();
        public VideoWatchStatus WatchStatus => this._folder.GetFolderStatus();

        public short Position { get; init; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void NotifyWatchStatus() => Notify(nameof(WatchStatus));

        public FolderItem(Folder folder)
        {
            this._folder = Guard.Against.Null(folder, nameof(folder));
        }
    }
}
