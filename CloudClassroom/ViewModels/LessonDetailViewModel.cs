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
                StartEndTime = $"{lessonModel.StartTime} - {lessonModel.EndTime}",
                LessonType = lessonModel.LessonType,
                CooperationType = lessonModel.CooperationType,
                Attendees = new ObservableCollection<UserModel>(),
            };

            LoadLessonDetailCommand = new DelegateCommand(async () =>
            {
                IList<UserModel> attendees = await WebApi.Instance.GetLessonAttendees((LessonType)LessonDetail.LessonType, LessonDetail.Id);

                LessonDetail.Attendees.Clear();

                if (attendees != null && attendees.Count > 0)
                {
                    foreach (var attendee in attendees)
                    {
                        LessonDetail.Attendees.Add(attendee);
                    }
                }
            });
        }
    }
}
