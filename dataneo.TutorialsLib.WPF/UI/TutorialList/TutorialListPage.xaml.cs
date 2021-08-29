using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using dataneo.TutorialLibs.WPF.UI.CategoryManage;
using dataneo.TutorialLibs.WPF.UI.Dialogs;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    /// <summary>
    /// Interaction logic for TutorialListPage.xaml
    /// </summary>
    public partial class TutorialListPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly TutorialListPageVM _tutorialListPageVM;

        public TutorialListPage(MainWindow mainWindow)
        {
            this._mainWindow = Guard.Against.Null(mainWindow, nameof(mainWindow));
            this._tutorialListPageVM = new TutorialListPageVM(mainWindow);
            InitializeComponent();
            this.DataContext = this._tutorialListPageVM;
        }

        public async Task LoadTutorialDtoAsync()
            => await this._tutorialListPageVM.LoadTutorialsDtoAsync(
                        this._tutorialListPageVM.SelectedTutorialsOrderType);

        private async void settingCategory_Click(object sender, System.Windows.RoutedEventArgs e)
            => await Result
                    .Try(() => new CategoryWindow(this._mainWindow).ShowCategory(), error => error.Message)
                    .OnFailure(error => ErrorWindow.ShowError(this._mainWindow, error));
    }
}
