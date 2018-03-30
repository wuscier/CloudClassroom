using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class FreeLoginViewModel:BindableBase
    {
        private readonly ISdk _sdk;
        private AuthResult _authResult;

        public FreeLoginViewModel()
        {
            RegisterCallbacks();

            SdkKey = "LgNeAkuV4Ns4MR9xeVtLLZMFwC7dffsdGrKI";
            SdkSecret = "eYjCcfZrgbULGGHLiRRsHTQSx84q8cQZlybj";

            Email = "2899989669@qq.com";
            Password = "justlucky";



            _sdk = ZoomSdk.Instance;

            SDKAuthCommand = new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(SdkKey))
                {
                    SdkAuthStatus = "请填写SDK key！";
                    return;
                };
                if (string.IsNullOrEmpty(SdkSecret))
                {
                    SdkAuthStatus = "请填写SDK secret！";
                    return;
                };

                 _sdk.SDKAuth(new AuthParam()
                {
                    appKey = SdkKey,
                    appSecret = SdkSecret,
                });

            });


            LoginCommand = new DelegateCommand(() =>
            {
                if (_authResult != AuthResult.AUTHRET_SUCCESS)
                {
                    LoginStatus = "请先确保SDK Auth成功！";
                }

                SDKError error = _sdk.Login(new LoginParam()
                {
                    loginType = LoginType.LoginType_Email,
                    emailLogin = new LoginParam4Email()
                    {
                        password = Password,
                        userName = Email,
                        bRememberMe = false,
                    }
                });
            });
        }

        private string _sdkKey;

        public string SdkKey
        {
            get { return _sdkKey; }
            set { SetProperty(ref _sdkKey, value); }
        }
        private string _sdkSecret;

        public string SdkSecret
        {
            get { return _sdkSecret; }
            set { SetProperty(ref _sdkSecret, value); }
        }
        private string _sdkAuthStatus;

        public string SdkAuthStatus
        {
            get { return _sdkAuthStatus; }
            set { SetProperty(ref _sdkAuthStatus, value); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _loginStatus;

        public string LoginStatus
        {
            get { return _loginStatus; }
            set { SetProperty(ref _loginStatus, value); }
        }

        public ICommand SDKAuthCommand { get; set; }
        public ICommand LoginCommand { get; set; }



        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onAuthenticationReturn((authResult) =>
            {
                _authResult = authResult;
                SdkAuthStatus = _authResult.ToString();
            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet((loginStatus, accountInfo) =>
            {
                System.Console.WriteLine(loginStatus.ToString());
                System.Console.WriteLine($"loginType:{accountInfo?.GetLoginType()}");
                System.Console.WriteLine($"displayName:{accountInfo?.GetDisplayName()}");

                LoginStatus = loginStatus.ToString();

                if (loginStatus == LOGINSTATUS.LOGIN_SUCCESS)
                {
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.FreeLoginView,
                    });

                }

            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(() =>
            {
                System.Console.WriteLine("log out");
            });
        }

    }
}
