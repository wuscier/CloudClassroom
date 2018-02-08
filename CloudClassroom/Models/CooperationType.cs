using System.ComponentModel;

namespace CloudClassroom.Models
{
    public enum CooperationType
    {
        [Description("校内")]
        InnerSchool,
        [Description("多校")]
        InterSchool,
    }
}
