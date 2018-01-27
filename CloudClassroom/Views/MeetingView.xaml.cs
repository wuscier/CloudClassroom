using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        private SubscriptionToken _showRecordPathToken;
        private SubscriptionToken _showSharingOptionsToken;


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

            _showRecordPathToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Subscribe((argument) =>
            {
                RecordPathView recordPathView = new RecordPathView();
                recordPathView.Owner = this;
                recordPathView.ShowDialog();
            });

            _showSharingOptionsToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Subscribe((argument) =>
            {
                SharingOptionsView sharingOptionsView = new SharingOptionsView();
                sharingOptionsView.Owner = this;
                sharingOptionsView.ShowDialog();
            });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Unsubscribe(_intoMeetingToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Unsubscribe(_videoPositionToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Unsubscribe(_showRecordPathToken);
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
            SyncMenuUI();
        }

        private void SyncVideoUI()
        {
            Win32APIs.MoveWindow(App.VideoHwnd, 0, 0, (int)video_container.ActualWidth, (int)video_container.ActualHeight, true);
        }

        private void SyncMenuUI()
        {
            if (bottom_menu_trigger.IsOpen)
            {
                var methodInfo = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                methodInfo.Invoke(bottom_menu_trigger, null);
            }

            if (bottom_menu.IsOpen)
            {
                var methodInfo = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                methodInfo.Invoke(bottom_menu, null);
            }
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            SyncMenuUI();
        }
    }
}
