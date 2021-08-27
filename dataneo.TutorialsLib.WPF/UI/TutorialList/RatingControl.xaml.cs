using dataneo.TutorialLibs.Domain.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace dataneo.TutorialLibs.WPF.UI.TutorialList
{
    /// <summary>
    /// Interaction logic for RatingStars.xaml
    /// </summary>
    public partial class RatingControl : UserControl
    {
        private static Brush Star = Brushes.Gold;
        private static Brush StarEmpty = Brushes.Gray;

        public static readonly DependencyProperty RatingStarProperty =
         DependencyProperty.Register(
             nameof(RatingStarValue),
             typeof(RatingStars),
             typeof(RatingControl),
             new PropertyMetadata(
                 RatingStars.NotRated,
                 new PropertyChangedCallback(OnStarredChanged)));

        public RatingStars RatingStarValue
        {
            get { return (RatingStars)GetValue(RatingStarProperty); }
            set { SetValue(RatingStarProperty, value); }
        }

        private static void OnStarredChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as RatingControl;
            ratingControl.OnSetRatingChanged(e);
        }

        private void OnSetRatingChanged(DependencyPropertyChangedEventArgs e)
        {
            this.RatingStarValue = (RatingStars)e.NewValue;
            SetRatingStatus(this.RatingStarValue);
        }

        public static readonly DependencyProperty TutorialGuidProperty =
        DependencyProperty.Register(
            nameof(TutorialGuid),
            typeof(int),
            typeof(RatingControl),
            new PropertyMetadata(
                default(int),
                new PropertyChangedCallback(OnTutorialGuidChanged)));

        public int TutorialGuid
        {
            get { return (int)GetValue(TutorialGuidProperty); }
            set { SetValue(TutorialGuidProperty, value); }
        }

        private static void OnTutorialGuidChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as RatingControl;
            ratingControl.OnSetTutorialGuidChanged(e);
        }

        private void OnSetTutorialGuidChanged(DependencyPropertyChangedEventArgs e)
        {
            this.TutorialGuid = (int)e.NewValue;
        }

        public static readonly DependencyProperty RatingStarChangedProperty =
         DependencyProperty.Register(
             nameof(RatingStarChanged),
             typeof(ICommand),
             typeof(RatingControl),
              new PropertyMetadata(null, new PropertyChangedCallback(OnRatingStarChangedChanged)));

        private static void OnRatingStarChangedChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            var ratingControl = d as RatingControl;
            ratingControl.OnSetRatingStarChangedChanged(e);
        }

        private void OnSetRatingStarChangedChanged(DependencyPropertyChangedEventArgs e)
        {
            this.RatingStarChanged = (ICommand)e.NewValue;
        }

        public ICommand RatingStarChanged
        {
            get { return (ICommand)GetValue(RatingStarChangedProperty); }
            set { SetValue(RatingStarChangedProperty, value); }
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

        private void SetAndInvokeRatingStarChanged(RatingStars ratingStars)
        {
            this.RatingStarValue = ratingStars;
            var value = new ValueTuple<int, RatingStars>(
                        this.TutorialGuid,
                        this.RatingStarValue);

            if (this.RatingStarChanged?.CanExecute(value) ?? false)
                this.RatingStarChanged.Execute(value);
        }

        private void scStar1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.OneStar);

        private void scStar_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(this.RatingStarValue);

        private void scStar2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.TwoStars);

        private void scStar3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.ThreeStart);

        private void scStar4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.FourStars);

        private void scStar5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            => this.SetRatingStatus(RatingStars.FiveStars);

        private void scStar1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetAndInvokeRatingStarChanged(RatingStars.OneStar);

        private void scStar2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetAndInvokeRatingStarChanged(RatingStars.TwoStars);

        private void scStar3_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetAndInvokeRatingStarChanged(RatingStars.ThreeStart);

        private void scStar4_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetAndInvokeRatingStarChanged(RatingStars.FourStars);

        private void scStar5_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetAndInvokeRatingStarChanged(RatingStars.FiveStars);
    }
}
