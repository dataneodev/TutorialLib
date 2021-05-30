using Ardalis.GuardClauses;
using LibVLCSharp.Shared;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dataneo.TutorialsLib.WPF.UI
{
    public class MediaPlayerAdapter : INotifyPropertyChanged
    {
        private readonly MediaPlayer _mediaPlayer;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int Volume
        {
            get { return this._mediaPlayer.Volume; }
            set
            {
                this._mediaPlayer.Volume = value;
                OnPropertyChanged();
            }
        }

        public byte Position
        {
            get { return (byte)(this._mediaPlayer.Position * 100); }
            set
            {
                this._mediaPlayer.Position = value / 100f;
                OnPropertyChanged();
            }
        }


        public MediaPlayerAdapter(MediaPlayer mediaPlayer)
        {
            this._mediaPlayer = Guard.Against.Null(mediaPlayer, nameof(mediaPlayer));

            this._mediaPlayer.PositionChanged += _mediaPlayer_PositionChanged;
            this._mediaPlayer.VolumeChanged += _mediaPlayer_VolumeChanged;
            this._mediaPlayer.Stopped += _mediaPlayer_Stopped;
            this._mediaPlayer.Paused += _mediaPlayer_Paused;
            this._mediaPlayer.Playing += _mediaPlayer_Playing;
        }

        private void _mediaPlayer_Playing(object sender, System.EventArgs e)
        {

        }

        private void _mediaPlayer_Paused(object sender, System.EventArgs e)
        {

        }

        private void _mediaPlayer_Stopped(object sender, System.EventArgs e)
        {

        }

        private void _mediaPlayer_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Volume));
        }

        private void _mediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Position));
        }
    }
}
