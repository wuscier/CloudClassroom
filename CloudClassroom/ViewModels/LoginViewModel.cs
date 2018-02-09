using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.Models;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Service;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class LoginModel : BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;

        public LoginModel()
        {
            RegisterCallbacks();

            LoginCommand = new DelegateCommand(async () =>
            {
                Logging = true;

                if (!IsInputFieldsValid())
                {
                    Logging = false;
                    return;
                }

                if (!(await APIAuth()))
                {
                    Logging = false;
                    return;
                }


                if (!(await GetUserInfo()))
                {
                    Logging = false;
                    return;
                }

                await WebApi.Instance.GetZoomUser(App.CurrentUser.Email);

                await SDKAuth();

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
                if (App.CurrentUser == null)
                {
                    App.CurrentUser = new UserModel()
                    {

                    };
                };

                App.CurrentUser.UserName = UserName;

                EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Publish(new EventArgument()
                {
                    Target = Target.LoginView,
                });
            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet((loginStatus, accountInfo) =>
            {
                System.Console.WriteLine(loginStatus.ToString());
                System.Console.WriteLine($"loginType:{accountInfo?.GetLoginType()}");
                System.Console.WriteLine($"displayName:{accountInfo?.GetDisplayName()}");
            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(() =>
            {
                System.Console.WriteLine("log out");
            });
        }

        private async Task<bool> APIAuth()
        {
            ResponseModel response = await WebApi.Instance.ApplyToken(UserName, Pwd);

            if (response.Status != 0)
            {
                Err = response.Message;
                return false;
            }

            return true;
        }

        private async Task<bool> GetUserInfo()
        {
            UserModel user = await WebApi.Instance.GetUserInfo();

            if (user == null)
            {
                Err = "获取用户信息失败！";
                return false;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                Err = "用户邮箱为空！";
                return false;
            }

            if (string.IsNullOrEmpty(user.AccountId))
            {
                Err = "用户账户为空！";
                return false;
            }

            App.CurrentUser = user;

            return true;
        }

        private async Task SDKAuth()
        {
            ZoomCredentialModel zoomInfo = await WebApi.Instance.GetZoomInfo();

            if (zoomInfo != null)
            {
                SDKError err = _sdk.SDKAuth(new AuthParam()
                {
                    appKey = zoomInfo.SdkKey,
                    appSecret = zoomInfo.SdkSecret,
                });

                if (err != SDKError.SDKERR_SUCCESS)
                {
                    Logging = false;
                    Err = SdkErrorTranslator.TranslateSDKError(err);
                }

                App.ZoomInfo = zoomInfo;
            }
            else
            {
                Logging = false;
                Err = "获取token,key和secret失败！";
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
