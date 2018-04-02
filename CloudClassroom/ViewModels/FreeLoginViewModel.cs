using CloudClassroom.Events;
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

        public FreeLoginViewModel()
        {
            RegisterCallbacks();

            Email = "2899989669@qq.com";
            Password = "justlucky";

            _sdk = ZoomSdk.Instance;

            LoginCommand = new DelegateCommand(() =>
            {
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

                LoginStatus = error.ToString();
            });
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

        public ICommand LoginCommand { get; set; }



        private void RegisterCallbacks()
        {
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
