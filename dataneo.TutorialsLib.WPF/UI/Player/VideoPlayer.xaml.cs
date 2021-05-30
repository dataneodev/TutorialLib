using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        private MediaPlayerAdapter mediaPlayerAdp;

        public MediaPlayerAdapter MediaPlayerAdp
        {
            get { return mediaPlayerAdp; }
            set { mediaPlayerAdp = value; OnPropertyChanged(); }
        }

        private string mediaPath = @"F:\Teledyski\Karolina Stanisławczyk - Cliché (official music video) (1080p_25fps_AV1-128kbit_AAC)_KjQYmiGcBKA.mp4";

        public string MediaPath
        {
            get { return mediaPath; }
            set { mediaPath = value; }
        }


        public VideoPlayer()
        {
            InitializeComponent();
            Loaded += VideoView_Loaded;
            Unloaded += Controls_Unloaded;
            this.DataContext = this;
        }

        private void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            _libVLC = new LibVLC(enableDebugLogs: true);
            _mediaPlayer = new MediaPlayer(_libVLC);
            this.MediaPlayerAdp = new MediaPlayerAdapter(_mediaPlayer);
            videoView.MediaPlayer = _mediaPlayer;
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnPlayPause.IsEnabled = false;
                if (_mediaPlayer.IsPlaying)
                {
                    _mediaPlayer.Pause();
                }
                else
                {

                    // if(_mediaPlayer.SetPause())
                    if (String.IsNullOrWhiteSpace(MediaPath))
                        return;

                    using (var media = new Media(_libVLC, new Uri(MediaPath)))
                    {
                        _mediaPlayer.Play(media);
                    }
                }

            }
            finally
            {
                btnPlayPause.IsEnabled = true;
            }
        }

        private void pbVideoProgress_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(pbVideoProgress).X;
            double ratio = MousePosition / pbVideoProgress.ActualWidth;
            this.pbVideoProgress.Value = ratio * pbVideoProgress.Maximum;
        }

        private void sVolume_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(sVolume).X;
            double ratio = MousePosition / sVolume.ActualWidth;
            this.sVolume.Value = ratio * sVolume.Maximum;
        }
    }
}
