using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CloudClassroom.Helpers
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            if (value != null)
            {
                brush.Color = (Color)ColorConverter.ConvertFromString(value.ToString());
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
