﻿using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TutorialsLib
{
    internal class PlayerWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IList<object> VideoItems { get; set; } = new List<object>();


        public ICommand ClickedOnEpisodeOrFolder;

        public PlayerWindowVM(Guid playedTutorialId)
        {
            VideoItems.Add(new FolderItem
            {
                Name = "Start",
                WatchStatus = VideoWatchStatus.Watched
            });

            VideoItems.Add(
                new VideoItem
                {
                    Name = "Test tytułu 1",
                    LocationOnList = VideoItemLocationType.FirstElement
                });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 2",
                LocationOnList = VideoItemLocationType.InnerElement
            });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 3",
                LocationOnList = VideoItemLocationType.LastElement
            });

            VideoItems.Add(new FolderItem
            {
                Name = "Folder test",
                WatchStatus = VideoWatchStatus.Watched
            });

            VideoItems.Add(
                new VideoItem
                {
                    Name = "Test tytułu 1",
                    LocationOnList = VideoItemLocationType.FirstElement,
                    WatchStatus = VideoWatchStatus.Watched
                });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 2",
                LocationOnList = VideoItemLocationType.InnerElement,
                WatchStatus = VideoWatchStatus.InProgress
            });

            VideoItems.Add(new VideoItem
            {
                Name = "Test tytułu 3",
                LocationOnList = VideoItemLocationType.LastElement,
                WatchStatus = VideoWatchStatus.NotWatched
            });

            VideoItems.Add(new FolderItem
            {
                Name = "Folder test",
                WatchStatus = VideoWatchStatus.NotWatched
            });
        }
    }
}
