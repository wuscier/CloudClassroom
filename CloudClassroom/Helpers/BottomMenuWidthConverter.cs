using System;
using System.Globalization;
using System.Windows.Data;

namespace CloudClassroom.Helpers
{
    public class BottomMenuWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value;
            return width <= 0 ? 0 : width - 50;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
