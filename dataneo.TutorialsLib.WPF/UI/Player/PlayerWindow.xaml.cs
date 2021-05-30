using Ardalis.GuardClauses;
using System;
using System.Windows;

namespace dataneo.TutorialsLib.WPF.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private readonly Action _onClose;
        private readonly PlayerWindowVM _VM;

        public PlayerWindow(Guid playedTutorialId, Action onClose)
        {
            this._onClose = Guard.Against.Null(onClose, nameof(onClose));
            InitializeComponent();
            this._VM = new PlayerWindowVM(playedTutorialId);
            this.DataContext = _VM;
        }

        public void Load()
        {
            this.Show();
            this._VM.Load();
        }

        private void playeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => this._onClose.Invoke();
    }
}
