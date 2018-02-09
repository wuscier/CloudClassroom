using CloudClassroom.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CloudClassroom.Helpers
{
    public class LessonTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intLessonType = (int)value;
            LessonType lessonType = (LessonType)intLessonType;
            string lessonTypeName = EnumHelper.GetEnumDesc(lessonType);
            return lessonTypeName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
