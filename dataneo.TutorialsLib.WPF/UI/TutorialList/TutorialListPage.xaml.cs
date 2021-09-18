using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    /// <summary>
    /// Interaction logic for TutorialListPage.xaml
    /// </summary>
    public partial class TutorialListPage : UserControl
    {
        public TutorialListPage()
        {
            InitializeComponent();
        }

        private async void settingCategory_Click(object sender, System.Windows.RoutedEventArgs e)
        { }
        //=> await Result
        //            .Try(() => new CategoryWindow(this._mainWindow).ShowCategory(), error => error.Message)
        //            .OnFailure(error => ErrorWindow.ShowError(this._mainWindow, error));
    }
}
