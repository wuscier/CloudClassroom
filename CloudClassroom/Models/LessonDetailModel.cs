using System.Collections.Generic;

namespace CloudClassroom.Models
{
    public class LessonDetailModel
    {
        public string Name { get; set; }
        public string StartEndTime { get; set; }
        public string LessonTypeName { get; set; }
        public string CooperationTypeName { get; set; }
        public string HostName { get; set; }
        public IList<UserModel> Attendees { get; set; }
    }
}
