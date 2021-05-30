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

        public PlayerWindow(Guid playedTutorialId, Action onClose)
        {
            this._onClose = Guard.Against.Null(onClose, nameof(onClose));

            InitializeComponent();
            var vm = new PlayerWindowVM(playedTutorialId);
            this.DataContext = vm;
            vm.Test();
            this.ShowDialog();
        }

        private void playeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => this._onClose.Invoke();
    }
}
