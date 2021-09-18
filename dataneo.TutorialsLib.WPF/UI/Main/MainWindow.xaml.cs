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
        //private readonly TutorialListPage _tutorialPage;
        //private readonly PlayerPage _playerPage;

        public MainWindow()
        {
            LibVLCSharp.Shared.Core.Initialize();
            //this._tutorialPage = new TutorialListPage();
            //this._playerPage = new PlayerPage(this);

            InitializeComponent();
            AppendWindowTitle();
            //LoadTutorialLibAsync();
        }

        private void AppendWindowTitle()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var assembly = Assembly.GetEntryAssembly()
                                   .GetCustomAttributes(typeof(AssemblyProductAttribute))
                                   .OfType<AssemblyProductAttribute>()
                                   .FirstOrDefault();

            this.Title = $"{assembly?.Product} - {version}";
        }

        public Task LoadTutorialLibAsync()
        {
            return Task.CompletedTask;
            //this.Content = this._tutorialPage;
            //await this._tutorialPage.LoadTutorialDtoAsync();
        }

        public Task PlayTutorialAsync(int idTutorial)
        {
            return Task.CompletedTask;
            //    this.Content = _playerPage;
            //    await _playerPage.LoadAsync(idTutorial);
        }

        private void tutorialWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //await Result.Try(() => this._playerPage.ClosingAsync());
        }
    }
}
