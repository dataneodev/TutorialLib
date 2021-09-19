using System.Linq;
using System.Reflection;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.UI
{
    /// <summary>
    /// Interaction logic for TutorialSelector.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            LibVLCSharp.Shared.Core.Initialize();
            InitializeComponent();
            AppendWindowTitle();
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
    }
}
