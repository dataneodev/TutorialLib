using System.Windows;

namespace dataneo.TutorialsLib.WPF.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public static void ShowError(Window parent, string title, string message)
        {

        }

        public ErrorWindow()
        {
            InitializeComponent();
        }
    }
}
