using System;
using System.Globalization;
using System.Windows.Data;

namespace CloudClassroom.Helpers
{
    public class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool originalValue = (bool)value;
            return !originalValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
