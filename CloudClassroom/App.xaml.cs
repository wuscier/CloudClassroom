using CloudClassroom.Helpers;
using CloudClassroom.Models;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Views;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static FreeLoginView FreeLoginView;

        public static MainView MainView;
        //public static BottomMenuView BottomMenuView;
        //public static BottomMenuViewModel BottomMenuViewModel;
        public static UserModel CurrentUser;
        public static ZoomCredentialModel ZoomInfo;
        public static uint MeetingUserId;

        //public static IntPtr BottomMenuViewHwnd = IntPtr.Zero;
        public static IntPtr MeetingViewHwnd = IntPtr.Zero;
        public static IntPtr VideoHwnd = IntPtr.Zero;

        public static ChargeMode ChargeMode = ChargeMode.Fee;

        private ISdk _sdk = ZoomSdk.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            CreateLogger();
            GetChargeMode();

            InitSDK();

            switch (ChargeMode)
            {
                case ChargeMode.Fee:

                    LoginView loginView = new LoginView();
                    loginView.Show();

                    break;
                case ChargeMode.Free:

                    FreeSDKAuthView freeSDKAuthView = new FreeSDKAuthView();
                    freeSDKAuthView.Show();
                    break;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;

            if (exception != null)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.ToString());
            }

            e.Handled = true;
        }

        private void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.RollingFile("Logs\\{Date}.log").CreateLogger();
        }

        private void GetChargeMode()
        {


            Assembly assembly = Assembly.GetExecutingAssembly();

            DirectoryInfo rootDir = Directory.GetParent(assembly.Location);

            string configFile = Path.Combine(rootDir.FullName, "ChargeMode.txt");

            if (File.Exists(configFile))
            {
                string sChargeMode = File.ReadAllText(configFile);

                if (!string.IsNullOrEmpty(sChargeMode))
                {
                    string trimmedChargeMode = sChargeMode.Trim();

                    try
                    {
                        ChargeMode chargeMode = (ChargeMode)Enum.Parse(typeof(ChargeMode), trimmedChargeMode);

                        App.ChargeMode = chargeMode;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex, "获取ChargeMode信息时出错！");
                    }
                }
            }
            else
            {
                File.WriteAllText(configFile, App.ChargeMode.ToString());
            }
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
                MessageBox.Show(SdkErrorTranslator.TranslateSDKError(err));
                Current.Shutdown();
            }
        }

        private void UninitSDK()
        {
            SDKError err = _sdk.CleanUp();

            if (err != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show(SdkErrorTranslator.TranslateSDKError(err));
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UninitSDK();
        }
    }
}
