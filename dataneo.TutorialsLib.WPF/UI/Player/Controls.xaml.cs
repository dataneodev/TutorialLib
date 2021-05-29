using LibVLCSharp.Shared;
using System;
using System.Windows;
using System.Windows.Controls;


namespace TutorialsLib
{
    /// <summary>
    /// Logika interakcji dla klasy Controls.xaml
    /// </summary>
    public partial class Controls : UserControl
    {
        readonly PlayerWindow parent;
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        public Controls(PlayerWindow Parent)
        {
            parent = Parent;

            InitializeComponent();

            // we need the VideoView to be fully loaded before setting a MediaPlayer on it.
            parent.VideoView.Loaded += VideoView_Loaded;
            PlayButton.Click += PlayButton_Click;
            StopButton.Click += StopButton_Click;
            Unloaded += Controls_Unloaded;
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

            parent.VideoView.MediaPlayer = _mediaPlayer;
        }

        void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (parent.VideoView.MediaPlayer.IsPlaying)
            {
                parent.VideoView.MediaPlayer.Stop();
            }
        }

        void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!parent.VideoView.MediaPlayer.IsPlaying)
            {
                using (var media = new Media(_libVLC, new Uri(@"F:\Teledyski\Karolina Stanisławczyk - Cliché (official music video) (1080p_25fps_AV1-128kbit_AAC)_KjQYmiGcBKA.mp4")))
                    parent.VideoView.MediaPlayer.Play(media);
            }
        }

    }
}
