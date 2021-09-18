using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dataneo.TutorialLibs.WPF.UI.Player
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
             typeof(PlayFileParameter),
             typeof(VideoPlayer),
             new PropertyMetadata(
                 null,
                 new PropertyChangedCallback(OnMediaPathChanged)));

        public PlayFileParameter MediaPath
        {
            get { return (PlayFileParameter)GetValue(MediaPathProperty); }
            set { SetValue(MediaPathProperty, value); }
        }

        private static void OnMediaPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var playerUC = d as VideoPlayer;
            playerUC.OnSetMediaPathChanged(e);
        }

        private void OnSetMediaPathChanged(DependencyPropertyChangedEventArgs e)
        {
            this.MediaPath = e.NewValue as PlayFileParameter;
            SetTimes(this.MediaPath.PlayTime);
            if (this.MediaPath is not null)
                PlayMediaFile(this.MediaPath);
        }

        public static readonly DependencyProperty PrevEpisodeProperty =
         DependencyProperty.Register(
             nameof(PrevEpisode),
             typeof(ICommand),
             typeof(VideoPlayer),
              new PropertyMetadata(null, new PropertyChangedCallback(OnPrevEpisodeChanged)));

        private static void OnPrevEpisodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as VideoPlayer;
            ratingControl.OnSetPrevEpisodeChanged(e);
        }

        private void OnSetPrevEpisodeChanged(DependencyPropertyChangedEventArgs e)
        {
            this.PrevEpisode = (ICommand)e.NewValue;
        }

        public ICommand PrevEpisode
        {
            get { return (ICommand)GetValue(PrevEpisodeProperty); }
            set { SetValue(PrevEpisodeProperty, value); }
        }

        public static readonly DependencyProperty NextEpisodeProperty =
         DependencyProperty.Register(
             nameof(NextEpisode),
             typeof(ICommand),
             typeof(VideoPlayer),
              new PropertyMetadata(null, new PropertyChangedCallback(OnNextEpisodeChanged)));

        private static void OnNextEpisodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as VideoPlayer;
            ratingControl.OnSetNextEpisodeChanged(e);
        }

        private void OnSetNextEpisodeChanged(DependencyPropertyChangedEventArgs e)
        {
            this.NextEpisode = (ICommand)e.NewValue;
        }

        public ICommand NextEpisode
        {
            get { return (ICommand)GetValue(NextEpisodeProperty); }
            set { SetValue(NextEpisodeProperty, value); }
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
            Loaded += VideoPlayer_Loaded;
        }

        public void StopPlaying()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                _mediaPlayer?.Stop();
            });
        }

        private void PlayMediaFile(PlayFileParameter mediaPath)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                WaitForLoad();
                using var media = new Media(_libVLC, new Uri(mediaPath.Path));
                this._mediaPlayer.Play(media);
                this._mediaPlayer.Position = mediaPath.Position / 100f;
            });
        }

        private void WaitForLoad()
        {
            while (!App.Current.Dispatcher.Invoke(() => IsLoaded))
            {
                Thread.Sleep(10);
            }
        }

        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._libVLC != null)
                return;

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
            UpdateTimes(newPosition);
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

        private void btnFullscreen_Click(object sender, RoutedEventArgs e)
            => SetFullscreenToggle();

        private TimeSpan playTime;
        private void SetTimes(TimeSpan playTime)
        {
            this.playTime = playTime;
        }

        private void UpdateTimes(int position)
        {
            App.Current.Dispatcher.Invoke(() =>
                tbTimeProgress.Text = GetNewTimeDescritpion(position));
        }

        private string GetNewTimeDescritpion(int position)
        {
            var playedTime = TimeSpan.FromSeconds(
                    (position / 100f) * this.playTime.TotalSeconds);

            return $"{GetFormatedTimeSpan(playedTime)} / {GetFormatedTimeSpan(this.playTime)}";
        }

        private string GetFormatedTimeSpan(TimeSpan timeSpan)
            => timeSpan.ToString(@"mm\:ss");

        WindowState toggleFullscreenOldState;
        private void SetFullscreenToggle()
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow is null)
                return;

            if (parentWindow.WindowState != WindowState.Maximized)
            {
                toggleFullscreenOldState = parentWindow.WindowState;
                parentWindow.WindowState = WindowState.Maximized;
                parentWindow.Visibility = Visibility.Collapsed;
                parentWindow.WindowStyle = WindowStyle.None;
                parentWindow.ResizeMode = ResizeMode.NoResize;
                parentWindow.Visibility = Visibility.Visible;
                parentWindow.Activate();
            }
            else
            {
                parentWindow.WindowState = toggleFullscreenOldState;
                parentWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                parentWindow.ResizeMode = ResizeMode.CanResize;
            }
        }
    }
}
