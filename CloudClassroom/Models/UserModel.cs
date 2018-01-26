namespace Classroom.Models
{
    public class UserModel
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }

        public string AccountUserId { get; set; }
        public string AccountUserName { get; set; }
        public uint InMeetingUserId { get; set; }
        public string InMeetingUserName { get; set; }
    }
}