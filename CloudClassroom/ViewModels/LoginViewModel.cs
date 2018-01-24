using Classroom.Models;
using CloudClassroom.Events;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class LoginModel : BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;

        public LoginModel()
        {
            LoginCommand = new DelegateCommand(() =>
            {
                Logging = true;

                if (!IsInputFieldsValid())
                {
                    Logging = false;
                    return;
                }

                RegisterCallbacks();

                SDKAuth();

            });
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _pwd;

        public string Pwd
        {
            get { return _pwd; }
            set { SetProperty(ref _pwd, value); }
        }

        private bool _autoLogin;

        public bool AutoLogin
        {
            get { return _autoLogin; }
            set { SetProperty(ref _autoLogin, value); }
        }

        private bool _rememberPwd;

        public bool RememberPwd
        {
            get { return _rememberPwd; }
            set { SetProperty(ref _rememberPwd, value); }
        }

        private string _err;
        public string Err
        {
            get { return _err; }
            set { SetProperty(ref _err, value); }
        }

        private bool _logging;
        public bool Logging
        {
            get { return _logging; }
            set { SetProperty(ref _logging, value); }
        }

        public ICommand LoginCommand { get; set; }

        private bool IsInputFieldsValid()
        {
            Err = string.Empty;
            if (string.IsNullOrEmpty(UserName))
            {
                EventAggregatorManager.Instance.EventAggregator.GetEvent<UIGotFocusEvent>().Publish(new EventArgument()
                {
                    Target = Target.LoginView,
                    Argument = new Argument() { Category = Category.UserName, }
                });
                Err = "请填写用户名！";
                return false;
            }

            if (string.IsNullOrEmpty(Pwd))
            {
                EventAggregatorManager.Instance.EventAggregator.GetEvent<UIGotFocusEvent>().Publish(new EventArgument()
                {
                    Target = Target.LoginView,
                    Argument = new Argument() { Category = Category.Password, }
                });
                Err = "请填写密码！";
                return false;
            }

            return true;
        }

        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onAuthenticationReturn((authResult) =>
            {
                if (authResult != AuthResult.AUTHRET_SUCCESS)
                {
                    Logging = false;
                    Err = authResult.ToString();
                    return;
                }

                Logging = false;
                if (App.UserModel == null)
                {
                    App.UserModel = new UserModel()
                    {

                    };
                };

                App.UserModel.UserName = UserName;


                EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Publish(new EventArgument()
                {
                    Target = Target.LoginView,
                });
            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet((loginStatus, accountInfo) =>
            {
                System.Console.WriteLine(loginStatus.ToString());
            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(() =>
            {
                System.Console.WriteLine("log out");
            });
        }


        private void SDKAuth()
        {
            SDKError err = _sdk.SDKAuth(new AuthParam()
            {
                appKey = "p3TojubkBYyntp8m4rVevr0yYmH1HVW9yPiR",
                appSecret = "JLuhz1VkcWGVSUESJj19biBi7NZcbVWENRXe",
            });

            if (err != SDKError.SDKERR_SUCCESS)
            {
                Logging = false;
                Err = err.ToString();
            }
        }
    }


    public class LoginViewModel
    {
        public LoginModel LoginModel { get; set; }

        public LoginViewModel()
        {
            LoginModel = new LoginModel()
            {
            };
        }
    }
}
