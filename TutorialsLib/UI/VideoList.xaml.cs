﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TutorialsLib.UI
{
    public partial class VideoList : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static readonly DependencyProperty SetItemSourceProperty =
         DependencyProperty.Register("ItemSource", typeof(IEnumerable<object>), typeof(VideoList), new
            PropertyMetadata(new object[0], new PropertyChangedCallback(OnItemSourcePropertyChanged)));

        public IEnumerable<object> ItemSource
        {
            get { return (IEnumerable<VideoItem>)GetValue(SetItemSourceProperty); }
            set { SetValue(SetItemSourceProperty, value); }
        }

        private static void OnItemSourcePropertyChanged(DependencyObject dependencyObject,
                DependencyPropertyChangedEventArgs e)
        {
            VideoList myUserControl = dependencyObject as VideoList;
            myUserControl.OnPropertyChanged("ItemSource");
            myUserControl.OnItemSourcePropertyChanged(e);
        }
        private void OnItemSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IEnumerable<object> newItemSource)
                tvVideoList.ItemsSource = newItemSource;
        }

        public VideoList()
        {
            InitializeComponent();
        }
    }
}
