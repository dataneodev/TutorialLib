using System;
using System.Windows.Data;

namespace dataneo.TutorialLibs.WPF.Converters
{
    public class LastPlayedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.Ticks == 1 ?
                    Translation.UI.NOT_WATCHED :
                    dateTime.ToString("dd.MM.yyyy hh:mm");
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
