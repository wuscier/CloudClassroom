using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Interop;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.Views
{
    /// <summary>
    /// MeetingView.xaml 的交互逻辑
    /// </summary>
    public partial class MeetingView : Window
    {
        private ProgressingControl _progressingControl;

        private SubscriptionToken _intoMeetingToken;
        private SubscriptionToken _videoPositionToken;

        private ISdk _sdk = ZoomSdk.Instance;


        public MeetingView()
        {
            InitializeComponent();
            SubscribeEvents();
            DataContext = new MeetingViewModel();
        }

        private void SubscribeEvents()
        {
            _intoMeetingToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Subscribe((argument) =>
            {
                _progressingControl?.Close();
                _progressingControl = null;
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _videoPositionToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Subscribe((argument) =>
            {
                SyncVideoUI();
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Unsubscribe(_intoMeetingToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Unsubscribe(_videoPositionToken);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.MeetingViewHwnd = new WindowInteropHelper(this).Handle;

            _progressingControl = new ProgressingControl();
            _progressingControl.Owner = this;
            _progressingControl.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            _sdk.Leave(LeaveMeetingCmd.LEAVE_MEETING);

            EventAggregatorManager.Instance.EventAggregator.GetEvent<LeaveMeetingEvent>().Publish(new EventArgument()
            {
                Target = Target.MainView,
            });
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SyncVideoUI();
        }

        private void SyncVideoUI()
        {
            Win32APIs.MoveWindow(App.VideoHwnd, 0, 0, (int)video_container.ActualWidth, (int)video_container.ActualHeight, true);
        }
    }
}
