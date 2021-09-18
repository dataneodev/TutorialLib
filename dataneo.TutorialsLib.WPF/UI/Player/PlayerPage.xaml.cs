using System.Windows;
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
            this.Loaded += UserControl1_Loaded;
            this.Unloaded += PlayerPage_Unloaded;
        }

        private void PlayerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing -= window_Closing;
        }

        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }

        private void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {

            this.ucVideoView?.StopPlaying();
            //await this._VM.EndWorkAsync();
            //    return Task.CompletedTask;
        }



        //public Task ClosingAsync()
        //{
        //   
        //}

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
