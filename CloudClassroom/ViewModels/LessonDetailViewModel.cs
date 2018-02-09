using CloudClassroom.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
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
            };

            LoadLessonDetailCommand = new DelegateCommand(async () =>
            {
                await Task.Delay(1000);
            });
        }
    }
}
