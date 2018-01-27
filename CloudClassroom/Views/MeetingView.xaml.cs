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
        private BottomMenuView _bottomMenuView;

        private SubscriptionToken _intoMeetingToken;
        private SubscriptionToken _videoPositionToken;
        private SubscriptionToken _showRecordPathToken;
        private SubscriptionToken _showSharingOptionsToken;
        private SubscriptionToken _showBottomMenuToken;


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

                _bottomMenuView = new BottomMenuView();
                _bottomMenuView.Show();

                Win32APIs.SetParent(new WindowInteropHelper(_bottomMenuView).Handle, App.VideoHwnd);
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
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _showSharingOptionsToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Subscribe((argument) =>
            {
                SharingOptionsView sharingOptionsView = new SharingOptionsView();
                sharingOptionsView.Owner = this;
                sharingOptionsView.ShowDialog();
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _showBottomMenuToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<BottomMenuLoadedEvent>().Subscribe((argument) =>
            {
                Win32APIs.SetParent((IntPtr)argument.Argument.Value, App.MeetingViewHwnd);
                

            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Unsubscribe(_intoMeetingToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Unsubscribe(_videoPositionToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Unsubscribe(_showRecordPathToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Unsubscribe(_showSharingOptionsToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<BottomMenuLoadedEvent>().Unsubscribe(_showBottomMenuToken);
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
            _bottomMenuView?.Close();
            _bottomMenuView = null;

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
