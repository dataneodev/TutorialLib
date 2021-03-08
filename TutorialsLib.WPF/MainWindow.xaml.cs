using System;
using System.Windows;

namespace TutorialsLib
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Controls _controls;
        public MainWindow()
        {
            LibVLCSharp.Shared.Core.Initialize();
            InitializeComponent();
            this.DataContext = new MainWindowVM();

            _controls = new Controls(this);
            VideoView.Content = _controls;

        }

        protected override void OnClosed(EventArgs e)
        {
            VideoView.Dispose();
        }

    }
}
