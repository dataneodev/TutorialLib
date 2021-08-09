using Ardalis.GuardClauses;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private readonly Action _onClose;
        private readonly PlayerWindowVM _VM;

        public PlayerWindow(int playedTutorialId, Action onClose)
        {
            this._onClose = Guard.Against.Null(onClose, nameof(onClose));
            InitializeComponent();
            this._VM = new PlayerWindowVM(this, playedTutorialId);
            this.DataContext = _VM;
            this.Show();
        }

        public Task LoadAsync() => this._VM.LoadAsync();

        private async void playeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await this._VM.EndWorkAsync();
            this._onClose.Invoke();
        }

        private void btToggleVideo_Click(object sender, RoutedEventArgs e)
        {
            var nextState = GetNextState();
            ucVideoList.Visibility = nextState;
            gsVerticalSpliter.Visibility = nextState;

            Visibility GetNextState()
                => ucVideoList.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
