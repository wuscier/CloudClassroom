using System.Windows.Input;

namespace CloudClassroom.Models
{
    public class LessonModel
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TeacherName { get; set; }
        public string HostId { get; set; }
        public string MeetingNumber { get; set; }
        public ICommand JoinCommand { get; set; }
    }
}
