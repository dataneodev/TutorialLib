using System.Windows;

namespace dataneo.TutorialsLibs.WPF.UI
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
            vm.LoadTutorialsDtoAsync(vm.SelectedTutorialsOrderType);
        }

        private void SetWindowVisibility(bool visible)
            => this.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
    }
}
