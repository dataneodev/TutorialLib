using dataneo.TutorialLibs.Domain.Tutorials;
using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoggerDialog.xaml
    /// </summary>
    public partial class LoggerDialog : UserControl, ILogger
    {
        public LoggerDialog()
        {
            InitializeComponent();
        }
    }
}
