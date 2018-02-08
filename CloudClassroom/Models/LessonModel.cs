using System.Windows.Input;

namespace CloudClassroom.Models
{
    public class LessonModel
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int LessonType { get; set; }
        public int CooperationType { get; set; }
        public string SpeakUserId { get; set; }
        public string MeetingId { get; set; }
        public bool Invalid { get; set; }

        public ICommand DetailCommand { get; set; }
        public ICommand JoinCommand { get; set; }
    }
}
