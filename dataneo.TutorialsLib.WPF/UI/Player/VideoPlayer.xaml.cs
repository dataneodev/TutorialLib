using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dataneo.TutorialsLib.WPF.UI
{
    public partial class VideoPlayer : UserControl, INotifyPropertyChanged
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region DependencyProperty
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
            if (!String.IsNullOrWhiteSpace(this.MediaPath))
                PlayMediaFile(this.MediaPath);
        }

        public static readonly DependencyProperty VideoEndedProperty =
         DependencyProperty.Register(
             nameof(VideoEnded),
             typeof(ICommand),
             typeof(VideoPlayer),
              new PropertyMetadata(null, new PropertyChangedCallback(OnVideoEndedChanged)));

        private static void OnVideoEndedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as VideoPlayer;
            ratingControl.OnSetVideoEndedChanged(e);
        }

        private void OnSetVideoEndedChanged(DependencyPropertyChangedEventArgs e)
        {
            this.VideoEnded = (ICommand)e.NewValue;
        }

        public ICommand VideoEnded
        {
            get { return (ICommand)GetValue(VideoEndedProperty); }
            set { SetValue(VideoEndedProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
         DependencyProperty.Register(
             nameof(Position),
             typeof(int),
             typeof(VideoPlayer),
              new PropertyMetadata(0, null));

        public int Position
        {
            get
            {
                return (int)GetValue(PositionProperty);
            }
            set
            {
                SetValue(PositionProperty, value);
                if (this._mediaPlayer != null)
                {
                    this._mediaPlayer.Position = value / 100f;
                }

                OnPropertyChanged();
            }
        }
        #endregion DependencyProperty

        private int volume;
        public int Volume
        {
            get { return this.volume; }
            set
            {
                this.volume = value;
                this._mediaPlayer.Volume = value;
                OnPropertyChanged();
            }
        }

        private PlayStatus playedStatus = PlayStatus.Stop;
        public PlayStatus PlayedStatus
        {
            get { return playedStatus; }
            set
            {
                switch (value)
                {
                    case PlayStatus.Play:
                        this._mediaPlayer.Play();
                        break;

                    case PlayStatus.Pause:
                        this._mediaPlayer.Pause();
                        break;

                    case PlayStatus.Stop:
                        this._mediaPlayer.Stop();
                        break;
                }

                playedStatus = value;
                OnPropertyChanged();
            }
        }

        public VideoPlayer()
        {
            InitializeComponent();
            this.DataContext = this;
            Unloaded += Controls_Unloaded;
            Loaded += VideoPlayer_Loaded;
        }

        private void PlayMediaFile(string mediaPath)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (this._mediaPlayer is not null)
                {
                    this._mediaPlayer.Stop();
                }

                var media = new Media(_libVLC, new Uri(mediaPath));
                this._mediaPlayer.Play(media);
            });
        }

        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            this._libVLC = new LibVLC();
            this._mediaPlayer = GetMediaPlayer(this._libVLC);

            videoView.MediaPlayer = this._mediaPlayer;
        }

        private MediaPlayer GetMediaPlayer(LibVLC libVLC)
        {
            var mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.PositionChanged += _mediaPlayer_PositionChanged;
            mediaPlayer.VolumeChanged += _mediaPlayer_VolumeChanged;
            mediaPlayer.Stopped += _mediaPlayer_Stopped;
            mediaPlayer.Paused += _mediaPlayer_Paused;
            mediaPlayer.Playing += _mediaPlayer_Playing;
            mediaPlayer.EndReached += _mediaPlayer_EndReached;
            return mediaPlayer;
        }

        private void Controls_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
        }

        private void _mediaPlayer_EndReached(object sender, System.EventArgs e)
        {
            this.playedStatus = PlayStatus.Stop;

            App.Current.Dispatcher.Invoke(() =>
            {
                OnPropertyChanged(nameof(PlayedStatus));
                if (this.VideoEnded?.CanExecute(null) ?? false)
                {
                    VideoEnded.Execute(null);
                }
            });
        }

        private void _mediaPlayer_Playing(object sender, System.EventArgs e)
        {
            this.playedStatus = PlayStatus.Play;
            OnPropertyChanged(nameof(PlayedStatus));
        }

        private void _mediaPlayer_Paused(object sender, System.EventArgs e)
        {
            this.playedStatus = PlayStatus.Pause;
            OnPropertyChanged(nameof(PlayedStatus));
        }

        private void _mediaPlayer_Stopped(object sender, System.EventArgs e)
        {
            this.playedStatus = PlayStatus.Stop;
            OnPropertyChanged(nameof(PlayedStatus));
        }

        private void _mediaPlayer_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            this.volume = (int)(e.Volume * 100);
            OnPropertyChanged(nameof(Volume));
        }

        private void _mediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            var newPosition = (int)(e.Position * 100);
            App.Current.Dispatcher.Invoke(() =>
            {
                SetValue(PositionProperty, newPosition);
                OnPropertyChanged(nameof(Position));
            });
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (this.PlayedStatus == PlayStatus.Play)
            {
                this.PlayedStatus = PlayStatus.Pause;
                return;
            }

            this.PlayedStatus = PlayStatus.Play;
        }

        private void pbVideoProgress_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(pbVideoProgress).X;
            double ratio = MousePosition / pbVideoProgress.ActualWidth;
            this.Position = (int)(ratio * pbVideoProgress.Maximum);
        }
    }
}
