using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.WPF.UI.Player;
using dataneo.TutorialLibs.WPF.UI.TutorialList;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.UI
{
    /// <summary>
    /// Interaction logic for TutorialSelector.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TutorialListPage _tutorialPage;
        private readonly PlayerPage _playerPage;

        public MainWindow()
        {
            LibVLCSharp.Shared.Core.Initialize();
            this._tutorialPage = new TutorialListPage(this);
            this._playerPage = new PlayerPage(this);

            InitializeComponent();
            AppendWindowTitle();
            LoadTutorialLibAsync();
        }

        private void AppendWindowTitle()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;

            var assembly = Assembly.GetEntryAssembly()
                                   .GetCustomAttributes(typeof(AssemblyProductAttribute))
                                   .OfType<AssemblyProductAttribute>()
                                   .FirstOrDefault();

            this.mainWindow.Title = $"{assembly.Product} - {version}";
        }

        public async Task LoadTutorialLibAsync()
        {
            this.Content = this._tutorialPage;
            await this._tutorialPage.LoadTutorialDtoAsync();
        }

        public async Task PlayTutorialAsync(int idTutorial)
        {
            this.Content = _playerPage;
            await _playerPage.LoadAsync(idTutorial);
        }

        private async void tutorialWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await Result.Try(() => this._playerPage.ClosingAsync());
        }
    }
}
