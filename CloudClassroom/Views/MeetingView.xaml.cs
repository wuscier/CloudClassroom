using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.ComponentModel;
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
        private bool _handledFirstMsg = false;


        public MeetingView()
        {
            InitializeComponent();
            RegisterCallbacks();
            SubscribeEvents();
        }

        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetUIHookControllerWrap().Add_CB_onUIActionNotify((type, msg) =>
            {
                Console.WriteLine($"type={type},msg={msg}");

                if (!_handledFirstMsg && type == UIHOOKHWNDTYPE.UIHOOKWNDTYPE_MAINWND && msg == 24)
                {
                    _handledFirstMsg = true;

                    HWNDDotNet first = new HWNDDotNet() { value = 0 };
                    HWNDDotNet second = new HWNDDotNet() { value = 0 };
                    _sdk.GetMeetingUIWnd(ref first, ref second);

                    App.VideoHwnd = new IntPtr(first.value);

                    Console.WriteLine($"video handle is:{App.VideoHwnd.ToInt32()}");

                    Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);
                    Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                    EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MeetingView,
                    });
                }
            });
        }

        private void SubscribeEvents()
        {
            _intoMeetingToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Subscribe((argument) =>
            {
                _progressingControl?.Close();
                _progressingControl = null;


                App.BottomMenuView = new BottomMenuView();
                App.BottomMenuView.DataContext = App.BottomMenuViewModel;

                App.BottomMenuView.Show();

                App.BottomMenuViewHwnd = new WindowInteropHelper(App.BottomMenuView).Handle;

                Console.WriteLine($"Bottom Menu View Handle：{App.BottomMenuViewHwnd}");

                Win32APIs.SetParent(App.BottomMenuViewHwnd, App.VideoHwnd);

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

        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Unsubscribe(_intoMeetingToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Unsubscribe(_videoPositionToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Unsubscribe(_showRecordPathToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Unsubscribe(_showSharingOptionsToken);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.MeetingViewHwnd = new WindowInteropHelper(this).Handle;

            _progressingControl = new ProgressingControl();
            _progressingControl.Owner = this;
            _progressingControl.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
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
