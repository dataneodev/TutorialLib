using System.Windows;

namespace dataneo.TutorialsLib.WPF.UI
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
            var vm = new MainWindowVM(this);
            vm.SetWindowVisibility = SetWindowVisibility;
            this.DataContext = vm;
            vm.LoadTutorialsDtoAsync();
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
