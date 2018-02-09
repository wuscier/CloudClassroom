﻿using System.Collections.ObjectModel;

namespace CloudClassroom.Models
{
    public class LessonDetailModel
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string StartEndTime { get; set; }
        public int LessonType { get; set; }
        public int CooperationType { get; set; }
        public string HostName { get; set; }

        public ObservableCollection<UserModel> Attendees { get; set; }
    }
}
