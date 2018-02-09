using CloudClassroom.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CloudClassroom.Helpers
{
    public class CooperationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intCooperationType = (int)value;

            CooperationType cooperationType = (CooperationType)intCooperationType;

            string cooperationName = EnumHelper.GetEnumDesc(cooperationType);

            return cooperationName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
