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

        private SubscriptionToken _selfVideoOnToken;
        private SubscriptionToken _videoUiAdaptedToken;

        private ISdk _sdk = ZoomSdk.Instance;


        public MeetingView()
        {
            InitializeComponent();
            SubscribeEvents();
            DataContext = new MeetingViewModel();
        }

        private void SubscribeEvents()
        {
            _selfVideoOnToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<SelfVideoOnEvent>().Subscribe((argument) =>
            {
                //Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);

                //Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                //SyncVideoUI();



                //Win32APIs.ShowWindowAsync(App.VideoHwnd, Win32APIs.SW_SHOW);

                _progressingControl?.Close();
                _progressingControl = null;
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _videoUiAdaptedToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<VideoUiAdaptedEvent>().Subscribe((argument) =>
            {

                SyncVideoUI();
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                    //bool result = Win32APIs.ShowWindowAsync(App.VideoHwnd, Win32APIs.SW_HIDE);
                    //result = Win32APIs.ShowWindowAsync(App.VideoHwnd, Win32APIs.SW_HIDE);

                    //Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);

                    //Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                    //SyncVideoUI();
                //}));



            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<SelfVideoOnEvent>().Unsubscribe(_selfVideoOnToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<VideoUiAdaptedEvent>().Unsubscribe(_videoUiAdaptedToken);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.MeetingViewHwnd = new WindowInteropHelper(this).Handle;

            //CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingConfiguration().SetMeetingUIPos(new WndPosition()
            //{
            //    hParent = new HWNDDotNet() { value = (uint)App.MeetingViewHwnd.ToInt32() },
            //    hSelfWnd = new HWNDDotNet() { value = (uint)App.VideoHwnd },
            //    left = 0,
            //    top = 0,
            //});

            //_progressingControl = new ProgressingControl();
            //_progressingControl.Owner = this;
            //_progressingControl.ShowDialog();
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
            //SyncVideoUI();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //SyncVideoUI();
        }

        private void SyncVideoUI()
        {
            Win32APIs.MoveWindow(App.VideoHwnd, 0, 0, (int)video_container.ActualWidth, (int)video_container.ActualHeight, true);
        }
    }
}
