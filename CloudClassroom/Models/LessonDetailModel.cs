using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CloudClassroom.Models
{
    public class LessonDetailModel:BindableBase
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int LessonType { get; set; }
        public int CooperationType { get; set; }

        private string _hostName;
        public string HostName
        {
            get { return _hostName; }
            set { SetProperty(ref _hostName, value); }
        }

        public string HostId { get; set; }

        public ObservableCollection<AttendeeModel> Attendees { get; set; }
    }
}
