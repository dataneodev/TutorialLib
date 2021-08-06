using dataneo.TutorialLibs.Domain.DTO;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace dataneo.TutorialsLibs.WPF.Converters
{
    public class ProgressBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TutorialHeaderDto tutorialHeaderDto)
            {
                var progress = ProgressBarValueConverter.GetProgressPercentage(tutorialHeaderDto);
                if (progress <= 25)
                    return Brushes.CornflowerBlue;

                if (progress > 25 && progress <= 50)
                    return Brushes.Gold;

                if (progress > 50 && progress <= 75)
                    return Brushes.Lime;

                if (progress > 75 && progress <= 90)
                    return Brushes.LimeGreen;

                if (progress > 90)
                    return Brushes.Green;
            }

            return Brushes.CornflowerBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
