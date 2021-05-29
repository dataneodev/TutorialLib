using System.Windows;

namespace dataneo.TutorialsLib.WPF.UI.Main
{
    /// <summary>
    /// Interaction logic for TutorialSelector.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowVM();
            vm.SetWindowVisibility = SetWindowVisibility;
            this.DataContext = vm;
        }

        private void SetWindowVisibility(bool visible)
        {
            if (visible)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
