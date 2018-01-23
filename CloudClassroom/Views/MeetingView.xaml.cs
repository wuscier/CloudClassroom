using CloudClassroom.CustomizedUI;
using CloudClassroom.ViewModels;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// MeetingView.xaml 的交互逻辑
    /// </summary>
    public partial class MeetingView : Window
    {
        private ProgressingControl _progressingControl;

        public MeetingView()
        {
            InitializeComponent();
            DataContext = new MeetingViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _progressingControl = new ProgressingControl();
            _progressingControl.Owner = this;
            _progressingControl.ShowDialog();
        }
    }
}
