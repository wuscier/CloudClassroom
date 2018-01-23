using CloudClassroom.CustomizedUI;
using CloudClassroom.ViewModels;
using System.Windows;
using System.Windows.Interop;

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
            App.MeetingViewHwnd = new WindowInteropHelper(this).Handle;

            _progressingControl = new ProgressingControl();
            _progressingControl.Owner = this;
            _progressingControl.ShowDialog();
        }
    }
}
