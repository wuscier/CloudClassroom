using CloudClassroom.ViewModels;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// MeetingView.xaml 的交互逻辑
    /// </summary>
    public partial class MeetingView : Window
    {
        public MeetingView()
        {
            InitializeComponent();
            DataContext = new MeetingViewModel();
        }
    }
}
