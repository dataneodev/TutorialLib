using Ardalis.GuardClauses;
using System;
using System.Windows;

namespace TutorialsLib
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private readonly Action _onClose;
        private readonly Controls _controls;
        public PlayerWindow(Guid playedTutorialId, Action onClose)
        {
            this._onClose = Guard.Against.Null(onClose, nameof(onClose));

            LibVLCSharp.Shared.Core.Initialize();
            InitializeComponent();
            this.DataContext = new PlayerWindowVM(playedTutorialId);

            _controls = new Controls(this);
            VideoView.Content = _controls;
            this.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            VideoView.Dispose();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void playeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => this._onClose.Invoke();
    }
}
