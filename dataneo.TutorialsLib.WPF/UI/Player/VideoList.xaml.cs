using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
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

        public static readonly DependencyProperty EpisodeClickProperty =
         DependencyProperty.Register(
             nameof(EpisodeClick),
             typeof(ICommand),
             typeof(VideoList),
              new PropertyMetadata(null, new PropertyChangedCallback(OnEpisodeClickChanged)));

        private static void OnEpisodeClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var videoList = d as VideoList;
            videoList.OnSetEpisodeClickChanged(e);
        }

        private void OnSetEpisodeClickChanged(DependencyPropertyChangedEventArgs e)
        {
            this.EpisodeClick = (ICommand)e.NewValue;
        }

        public ICommand EpisodeClick
        {
            get { return (ICommand)GetValue(EpisodeClickProperty); }
            set { SetValue(EpisodeClickProperty, value); }
        }

        public VideoList()
        {
            InitializeComponent();
        }

        private void tvVideoList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is VideoItem)
            {
                var selectedItem = listView.SelectedItem as VideoItem;
                if (this.EpisodeClick?.CanExecute(selectedItem.EpisodeId) ?? false)
                    this.EpisodeClick?.Execute(selectedItem.EpisodeId);
            }
        }
    }
}
