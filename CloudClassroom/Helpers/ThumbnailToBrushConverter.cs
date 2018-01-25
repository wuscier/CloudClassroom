using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CloudClassroom.Helpers
{
    public class ThumbnailToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uri = value as string;
            ImageBrush imageBrush = null;

            if (!string.IsNullOrEmpty(uri))
            {
                imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CloudClassroom;component/" + uri, UriKind.RelativeOrAbsolute));
            }

            return imageBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
