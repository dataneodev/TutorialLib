using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Core.Initialize();
            InitializeComponent();

            _controls = new Controls(this);
            VideoView.Content = _controls;

        }

        protected override void OnClosed(EventArgs e)
        {
            VideoView.Dispose();
        }

    }
}
