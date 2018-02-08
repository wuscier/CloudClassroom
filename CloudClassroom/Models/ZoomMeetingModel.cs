namespace CloudClassroom.Models
{
    public class ZoomMeetingModel
    {
        public string MeetingId { get; set; }
        public string HostId { get; set; }
        public string Topic { get; set; }
        public string Password { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        public string JoinUrl { get; set; }
    }
}
