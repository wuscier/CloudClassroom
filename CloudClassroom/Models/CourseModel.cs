using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudClassroom.Models
{
    public class CourseModel
    {
        public string Duration { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public string HostId { get; set; }
        public string MeetingNumber { get; set; }
        public ICommand JoinCommand { get; set; }
    }
}
