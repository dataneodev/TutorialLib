using System.Threading.Tasks;
using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    /// <summary>
    /// Interaction logic for TutorialListPage.xaml
    /// </summary>
    public partial class TutorialListPage : Page
    {
        private readonly TutorialListPageVM _tutorialListPageVM;

        public TutorialListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this._tutorialListPageVM = new TutorialListPageVM(mainWindow);
            this.DataContext = this._tutorialListPageVM;
        }

        public async Task LoadTutorialDtoAsync()
        {
            await this._tutorialListPageVM.LoadTutorialsDtoAsync(
                      this._tutorialListPageVM.SelectedTutorialsOrderType);
        }
    }
}
