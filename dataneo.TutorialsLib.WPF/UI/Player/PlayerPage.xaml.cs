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

        private void btToggleVideo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ucVideoList.Visibility = (ucVideoList.Visibility == System.Windows.Visibility.Visible) ?
                                        System.Windows.Visibility.Collapsed :
                                        System.Windows.Visibility.Visible;
        }

        private void btnBackToTutorialList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ucVideoView.StopPlaying();
        }
    }
}
