using CloudClassroom.Models;
using CloudClassroom.Service;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CloudClassroom.ViewModels
{
    public class LessonDetailViewModel : BindableBase
    {

        private LessonDetailModel _lessonDetail;
        public LessonDetailModel LessonDetail
        {
            get { return _lessonDetail; }
            set
            {
                SetProperty(ref _lessonDetail, value);
            }
        }

        public ICommand LoadLessonDetailCommand { get; set; }


        public LessonDetailViewModel(LessonModel lessonModel)
        {
            LessonDetail = new LessonDetailModel()
            {
                Id = lessonModel.Id,
                Name = lessonModel.Name,
                StartTime = lessonModel.StartTime,
                EndTime = lessonModel.EndTime,
                LessonType = lessonModel.LessonType,
                CooperationType = lessonModel.CooperationType,
                Attendees = new ObservableCollection<AttendeeModel>(),
                HostId = lessonModel.SpeakUserId,
            };

            LoadLessonDetailCommand = new DelegateCommand(async () =>
            {
                IList<AttendeeModel> attendees = await WebApi.Instance.GetLessonAttendees((LessonType)LessonDetail.LessonType, LessonDetail.Id);

                LessonDetail.Attendees.Clear();

                if (attendees != null && attendees.Count > 0)
                {
                    foreach (var attendee in attendees)
                    {
                        if (attendee.UserId == LessonDetail.HostId)
                        {
                            LessonDetail.HostName = attendee.Name;
                            continue;
                        }

                        LessonDetail.Attendees.Add(attendee);
                    }
                }
            });
        }
    }
}
