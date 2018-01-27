using CloudClassroom.Events;
using System.Windows;
using System.Windows.Interop;

namespace CloudClassroom.Views
{
    /// <summary>
    /// BottomMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class BottomMenuView : Window
    {
        public BottomMenuView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //EventAggregatorManager.Instance.EventAggregator.GetEvent<BottomMenuLoadedEvent>().Publish(new EventArgument()
            //{
            //    Argument = new Argument()
            //    {
            //        Value = new WindowInteropHelper(this).Handle,
            //    },
            //    Target = Target.MeetingView,
            //});
        }
    }
}
