using Ardalis.GuardClauses;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.UI.Dialogs
{
    public partial class ErrorWindow : Window
    {
        public static void ShowError(Window parent, string message)
        {
            Guard.Against.Null(parent, nameof(parent));
            var errorWindow = new ErrorWindow()
            {
                Owner = parent
            };
            errorWindow.tbErrorMessage.Text = message;
            errorWindow.Show();
        }

        public ErrorWindow()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
            => this.Close();
    }
}
