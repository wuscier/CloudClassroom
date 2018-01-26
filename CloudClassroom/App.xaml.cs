using Classroom.Models;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Views;
using Serilog;
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
        public static bool IsHost = false;

        private ISdk _sdk = ZoomSdk.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            CreateLogger();

            InitSDK();
        }

        private void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.RollingFile("Logs\\{Date}.log").CreateLogger();
        }

        private void InitSDK()
        {
            SDKError err = _sdk.Initialize(new InitParam()
            {
                brand_name = "云课堂",
                web_domain = "https://zoom.us",
                language_id = SDK_LANGUAGE_ID.LANGUAGE_Chinese_Simplified,

            });

            if (err != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show(Translator.TranslateSDKError(err));
                Current.Shutdown();
            }
        }

        private void UninitSDK()
        {
            SDKError err = _sdk.CleanUp();

            if (err != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show(Translator.TranslateSDKError(err));
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UninitSDK();
        }
    }
}
