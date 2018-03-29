using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.ComponentModel;
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
        private SubscriptionToken _showRecordPathToken;
        private SubscriptionToken _showSharingOptionsToken;
        private SubscriptionToken _resetVideoUiToken;
        private SubscriptionToken _visibleToken;
        private SubscriptionToken _fullscreenToken;

        private ISdk _sdk = ZoomSdk.Instance;
        private bool _handledFirstMsg = false;


        public BottomMenuViewModel BottomMenuViewModel { get; set; }


        public MeetingView()
        {
            InitializeComponent();



            RegisterCallbacks();
            SubscribeEvents();

            InitData();
        }

        private void InitData()
        {
            BottomMenuViewModel = new BottomMenuViewModel();
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

                    Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                    MoveVideoUI();
                }
            });
        }

        private void SubscribeEvents()
        {
            _intoMeetingToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Subscribe((argument) =>
            {
                _progressingControl?.Close();
                _progressingControl = null;

                //MouseHook.Start(MouseHookHandler);

                //InitBottomMenu();
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _resetVideoUiToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<ResetVideoUiEvent>().Subscribe((argument) =>
            {
                MoveVideoUI();
            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _visibleToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<MeetingViewVisibleEvent>().Subscribe((argument) =>
            {

                switch (argument.Argument.Category)
                {
                    case Category.Hide:
                        Hide();
                        break;

                    case Category.Show:
                        IntPtr sharedWndHandle = Win32APIs.FindWindow(null, "共享白板");

                        if (sharedWndHandle != IntPtr.Zero)
                        {
                            Win32APIs.SendNotifyMessage(sharedWndHandle, 16, 0, IntPtr.Zero);
                        }

                        //App.BottomMenuView.Visibility = Visibility.Collapsed;
                        Show();
                        break;

                }

            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MeetingView; });

            _fullscreenToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<FullScreenStatusEvent>().Subscribe((argument) =>
            {

                switch (argument.Argument.Category)
                {
                    case Category.Enter:

                        EnterFullScreen();

                        break;

                    case Category.Exit:

                        ExitFullScreen();

                        break;
                }

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

            EventAggregatorManager.Instance.EventAggregator.GetEvent<MeetingViewVisibleEvent>().Unsubscribe(_visibleToken);

            EventAggregatorManager.Instance.EventAggregator.GetEvent<ResetVideoUiEvent>().Unsubscribe(_resetVideoUiToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Unsubscribe(_showRecordPathToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Unsubscribe(_showSharingOptionsToken);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.MeetingViewHwnd = new WindowInteropHelper(this).Handle;

            _progressingControl = new ProgressingControl();

            Point screenPoint = video_container.PointToScreen(new Point() { X = 0, Y = 0 });

            _progressingControl.Left = (video_container.ActualWidth - _progressingControl.Width) / 2 + screenPoint.X;
            _progressingControl.Top = (video_container.ActualHeight - _progressingControl.Height) / 2 + screenPoint.Y;
            _progressingControl.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MouseHook.Stop();

            UnsubscribeEvents();

            _sdk.Leave(LeaveMeetingCmd.LEAVE_MEETING);

            App.MainView.Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveVideoUI();
            //MoveBottomMenu();
        }

        private void MoveVideoUI()
        {
            Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);

            Win32APIs.MoveWindow(App.VideoHwnd, 0, 0, (int)video_container.ActualWidth, (int)video_container.ActualHeight, true);
        }

        private void EnterFullScreen()
        {
            chat_area.Visibility = Visibility.Collapsed;
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            MoveVideoUI();
        }

        private void ExitFullScreen()
        {
            chat_area.Visibility = Visibility.Visible;
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
            MoveVideoUI();
        }

        //private void MoveBottomMenu()
        //{
        //    Win32APIs.MoveWindow(App.BottomMenuViewHwnd, 0, (int)(video_container.ActualHeight - 100), (int)video_container.ActualWidth, 100, true);
        //}

        //private void InitBottomMenu()
        //{
        //App.BottomMenuView = new BottomMenuView();
        //App.BottomMenuView.DataContext = App.BottomMenuViewModel;

        //App.BottomMenuView.Show();

        //App.BottomMenuViewHwnd = new WindowInteropHelper(App.BottomMenuView).Handle;

        //Win32APIs.SetParent(App.BottomMenuViewHwnd, App.VideoHwnd);
        //    MoveBottomMenu();
        //}

        //private int MouseHookHandler(int code, Int32 wParam, IntPtr lParam)
        //{
        //    if (wParam == 512)
        //    {
        //        Win32APIs.MouseHookStruct mouseHookStruct = Marshal.PtrToStructure<Win32APIs.MouseHookStruct>(lParam);

        //        Point videoPoint = video_container.PointToScreen(new Point() { X = 0, Y = 0 });

        //        if (mouseHookStruct.pt.x >= videoPoint.X && mouseHookStruct.pt.x <= videoPoint.X + video_container.ActualWidth && mouseHookStruct.pt.y >= videoPoint.Y && mouseHookStruct.pt.y <= videoPoint.Y + video_container.ActualHeight)
        //        {
        //            if (App.BottomMenuView.Visibility != Visibility.Visible && App.BottomMenuView.IsLoaded)
        //            {
        //                App.BottomMenuView.Visibility = Visibility.Visible;
        //            }
        //        }
        //        else
        //        {
        //            if (App.BottomMenuView.Visibility != Visibility.Collapsed && App.BottomMenuView.IsLoaded)
        //            {
        //                App.BottomMenuView.Visibility = Visibility.Collapsed;
        //            }
        //        }
        //    }

        //    return MouseHook.CallNextHookEx(code, wParam, lParam);
        //}
    }
}
