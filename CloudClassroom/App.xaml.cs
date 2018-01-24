using Classroom.Models;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Views;
using System;
using System.Windows;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static MainView MainView;
        public static UserModel UserModel;
        public static IntPtr MeetingViewHwnd = IntPtr.Zero;
        public static IntPtr VideoHwnd = IntPtr.Zero;

        private ISdk _sdk = ZoomSdk.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            SDKError err = _sdk.Initialize(new InitParam()
            {
                brand_name = "云课堂",
                web_domain = "https://zoom.us",
                language_id = SDK_LANGUAGE_ID.LANGUAGE_Chinese_Simplified,

            });

            if (err != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show("服务初始化失败！");
                return;
            }
            //else
            //{
            //    //SDKError monitorWndError = _sdk.MonitorWnd(_sdk.MeetingUiClassName, true);
            //    //SDKError monitorWndMsgError = _sdk.MonitorWndMessage(Win32APIs.WM_SHOWWINDOW, true);
                SDKError startMonitorError = _sdk.StartMonitor();
            //CMeetingConfigurationDotNetWrap.Instance.SetBottomFloatToolbarWndVisibility(false);
            //}
        }

        protected override void OnExit(ExitEventArgs e)
        {
           SDKError stopMonitorError = _sdk.StopMonitor();

            SDKError err = _sdk.CleanUp();

            if (err != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show("服务清理失败！");
            }

        }

    }
}
