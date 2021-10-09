using dataneo.TutorialLibs.Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace dataneo.TutorialLibs.WPF.Converters
{
    public class CategoriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IReadOnlyList<Category> categories)
            {
                if (categories.Count == 0)
                    return "-";

                return string.Join(" | ", categories.Select(s => s.Name));
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
