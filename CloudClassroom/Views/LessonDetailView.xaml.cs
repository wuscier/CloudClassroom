using CloudClassroom.Models;
using CloudClassroom.ViewModels;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// LessonDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class LessonDetailView : Window
    {
        public LessonDetailView(LessonModel lessonModel)
        {
            InitializeComponent();
            DataContext = new LessonDetailViewModel(lessonModel);
        }
    }
}
