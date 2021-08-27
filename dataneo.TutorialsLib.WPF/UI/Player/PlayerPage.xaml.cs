using System.Threading.Tasks;
using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.Player
{
    /// <summary>
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : Page
    {
        private readonly PlayerPageVM _VM;

        public PlayerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this._VM = new PlayerPageVM(mainWindow);
            this.DataContext = _VM;
        }

        public async Task LoadAsync(int tutorialId)
        {
            await this._VM.LoadAsync(tutorialId);
        }

        private async void playeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await this._VM.EndWorkAsync();
        }

        private void btToggleVideo_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private async void btnBackToTutorialList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ucVideoView.StopPlaying();
            await this._VM.GoBackToTutorialListCommandAsync();
        }
    }
}
