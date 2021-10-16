using System;
using System.Windows.Controls;

namespace dataneo.TutorialLibs.WPF.UI.Player
{
    /// <summary>
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : UserControl
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        private double gcVideoListWidth;
        private void btToggleVideo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (gcVideoList.Width.Value == 0)
            {
                gcVideoList.Width = new System.Windows.GridLength(Math.Max(gcVideoListWidth, 100));
                return;
            }

            gcVideoListWidth = gcVideoList.Width.Value;
            gcVideoList.Width = new System.Windows.GridLength(0);
        }
    }
}
