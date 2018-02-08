using System.ComponentModel;

namespace CloudClassroom.Models
{
    public enum LessonType
    {
        [Description("互动课程")]
        Interactive,
        [Description("直播课程")]
        Live,
        [Description("")]
        Vod,
        [Description("")]
        Resource,
        [Description("未知")]
        Unknown,
    }
}
