using dataneo.TutorialLibs.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dataneo.TutorialsLib.WPF.UI.Main
{
    /// <summary>
    /// Interaction logic for RatingStars.xaml
    /// </summary>
    public partial class RatingControl : UserControl
    {
        private static Brush Star = Brushes.Yellow;
        private static Brush StarEmpty = Brushes.Gray;

        public static readonly DependencyProperty RatingProperty =
         DependencyProperty.Register("Rating", typeof(RatingStars), typeof(StarControl), new
            PropertyMetadata("", new PropertyChangedCallback(OnStarredChanged)));

        public RatingStars Rating
        {
            get { return (RatingStars)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void OnStarredChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            RatingControl UserControl1Control = d as RatingControl;
            UserControl1Control.OnSetTextChanged(e);
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        public RatingControl()
        {
            InitializeComponent();
        }

        private void SetRatingStatus(RatingStars ratingStars)
        {
            var value = (byte)ratingStars;

            if (value > 0)
                scStar1.pathStart.Fill = Star;
            else
                scStar1.pathStart.Fill = StarEmpty;

            if (value > 1)
                scStar2.pathStart.Fill = Star;
            else
                scStar2.pathStart.Fill = StarEmpty;

            if (value > 2)
                scStar3.pathStart.Fill = Star;
            else
                scStar3.pathStart.Fill = StarEmpty;

            if (value > 3)
                scStar4.pathStart.Fill = Star;
            else
                scStar4.pathStart.Fill = StarEmpty;

            if (value > 4)
                scStar5.pathStart.Fill = Star;
            else
                scStar5.pathStart.Fill = StarEmpty;
        }

        private void scStar1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.OneStar);

        private void scStar_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(this.Rating);

        private void scStar2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.TwoStars);

        private void scStar3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.ThreeStart);

        private void scStar4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.FourStars);

        private void scStar5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.FiveStars);
    }
}
