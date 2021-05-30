using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace dataneo.TutorialsLib.WPF.UI
{
    /// <summary>
    /// Logika interakcji dla klasy VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl, INotifyPropertyChanged
    {
        private readonly LibVLC _libVLC;
        private readonly MediaPlayer _mediaPlayer;
        private MediaPlayerAdapter _mediaPlayerAdp;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MediaPlayerAdapter MediaPlayerAdp => _mediaPlayerAdp;

        public static readonly DependencyProperty MediaPathProperty =
         DependencyProperty.Register(
             nameof(MediaPath),
             typeof(string),
             typeof(VideoPlayer),
             new PropertyMetadata(
                 String.Empty,
                 new PropertyChangedCallback(OnMediaPathChanged)));
        public string MediaPath
        {
            get { return (string)GetValue(MediaPathProperty); }
            set { SetValue(MediaPathProperty, value); }
        }

        private static void OnMediaPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var playerUC = d as VideoPlayer;
            playerUC.OnSetMediaPathChanged(e);
        }

        private void OnSetMediaPathChanged(DependencyPropertyChangedEventArgs e)
        {
            this.MediaPath = (string)e.NewValue;
            if (File.Exists(this.MediaPath))
                PlayMediaFile(this.MediaPath);
        }

        public VideoPlayer()
        {
            this._libVLC = new LibVLC();
            this._mediaPlayer = new MediaPlayer(_libVLC);
            this._mediaPlayerAdp = new MediaPlayerAdapter(_mediaPlayer);

            InitializeComponent();
            videoView.MediaPlayer = _mediaPlayer;
            Unloaded += Controls_Unloaded;
            Loaded += VideoPlayer_Loaded;
            this.DataContext = this;
        }

        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayerAdp = null;
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
        }

        private void PlayMediaFile(string mediaPath)
        {
            using (var media = new Media(_libVLC, new Uri(mediaPath)))
            {
                _mediaPlayer.Play(media);
            }
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            btnPlayPause.IsEnabled = false;
            if (this._mediaPlayerAdp.PlayedStatus == PlayStatus.Play)
            {
                this._mediaPlayerAdp.PlayedStatus = PlayStatus.Pause;
                return;
            }
            btnPlayPause.IsEnabled = true;
        }

        private void pbVideoProgress_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(pbVideoProgress).X;
            double ratio = MousePosition / pbVideoProgress.ActualWidth;
            this.pbVideoProgress.Value = ratio * pbVideoProgress.Maximum;
        }
    }
}
