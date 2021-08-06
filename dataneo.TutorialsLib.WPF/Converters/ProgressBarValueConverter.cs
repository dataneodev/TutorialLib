using dataneo.Extensions;
using dataneo.TutorialLibs.Domain.DTO;
using System;
using System.Globalization;
using System.Windows.Data;

namespace dataneo.TutorialsLibs.WPF.Converters
{
    public class ProgressBarValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TutorialHeaderDto tutorialHeaderDto)
            {
                return GetProgressPercentage(tutorialHeaderDto).ToString("0");
            }
            return 0.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static int GetProgressPercentage(TutorialHeaderDto tutorialHeaderDto)
        {
            if (tutorialHeaderDto == null)
                return 0;

            if (tutorialHeaderDto.TotalTime.TotalSeconds.EqualsPrecision(0, 0.1))
                return 0;

            var progress = (int)Math.Round(100 * tutorialHeaderDto.TimePlayed.TotalSeconds / tutorialHeaderDto.TotalTime.TotalSeconds);

            if (progress > 100)
                return 100;

            if (progress < 0)
                return 0;

            return progress;
        }
    }
}
