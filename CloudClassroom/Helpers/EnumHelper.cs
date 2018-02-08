using System;
using System.ComponentModel;
using System.Reflection;

namespace CloudClassroom.Helpers
{
    public class EnumHelper
    {
        public static string GetEnumDesc(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memberInfo = type.GetMember(en.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
