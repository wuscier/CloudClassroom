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

        private LOGINSTATUS _loginCBStatus = LOGINSTATUS.LOGIN_IDLE;

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

            StartMeetingCommand = new DelegateCommand(() =>
            {
                if (_loginCBStatus != LOGINSTATUS.LOGIN_SUCCESS)
                {
                    StartMeetingStatus = "请先确保登录成功！";
                    return;
                }

                ulong meetingId;
                if (!ulong.TryParse(HostStartMeetingId, out meetingId))
                {
                    StartMeetingStatus = "请输入有效的会议号！";
                    return;
                }

                SDKError error = _sdk.Start(new StartParam()
                {
                    userType = SDKUserType.SDK_UT_NORMALUSER,
                    normaluserStart = new StartParam4NormalUser()
                    {
                        hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                        isAudioOff = false,
                        isDirectShareDesktop = false,
                        isVideoOff = false,
                        meetingNumber = meetingId,
                        participantId = string.Empty,
                        
                    }
                });

                if (error == SDKError.SDKERR_SUCCESS)
                {
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.FreeLoginView,
                    });
                }
                else
                {
                    StartMeetingStatus = error.ToString();
                }
            });


            JoinMeetingCommand = new DelegateCommand(() =>
            {
                ulong meetingId;
                if (!ulong.TryParse(AttendeeJoinMeetingId, out meetingId))
                {
                    JoinMeetingStatus = "请输入有效的会议号！";
                    return;
                }

                SDKError error = _sdk.Join(new JoinParam()
                {
                    userType = SDKUserType.SDK_UT_NORMALUSER,
                    normaluserJoin = new JoinParam4NormalUser()
                    {
                        hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                        isAudioOff = false,
                        isDirectShareDesktop = false,
                        isVideoOff = false,
                        meetingNumber = meetingId,
                        participantId = string.Empty,
                        psw = MeetingPassword,
                        userName = ParticipantName,
                        webinarToken = string.Empty,
                    }
                });

                if (error == SDKError.SDKERR_SUCCESS)
                {
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.FreeLoginView,
                    });
                }
                else
                {
                    JoinMeetingStatus = error.ToString();
                }

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



        private string _hostStartMeetingId;

        public string HostStartMeetingId
        {
            get { return _hostStartMeetingId; }
            set { SetProperty(ref _hostStartMeetingId, value); }
        }



        private string _participantName;

        public string ParticipantName
        {
            get { return _participantName; }
            set { SetProperty(ref _participantName, value); }
        }




        private string _attendeeJoinMeetingId;

        public string AttendeeJoinMeetingId
        {
            get { return _attendeeJoinMeetingId; }
            set { SetProperty(ref _attendeeJoinMeetingId, value); }
        }




        private string _meetingPassword;

        public string MeetingPassword
        {
            get { return _meetingPassword; }
            set { SetProperty(ref _meetingPassword, value); }
        }



        private string _loginStatus;

        public string LoginStatus
        {
            get { return _loginStatus; }
            set { SetProperty(ref _loginStatus, value); }
        }

        private string _startMeetingStatus;

        public string StartMeetingStatus
        {
            get { return _startMeetingStatus; }
            set { SetProperty(ref _startMeetingStatus, value); }
        }


        private string _joinMeetingStatus;

        public string JoinMeetingStatus
        {
            get { return _joinMeetingStatus; }
            set { SetProperty(ref _joinMeetingStatus, value); }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand StartMeetingCommand { get; set; }
        public ICommand JoinMeetingCommand { get; set; }



        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet((loginStatus, accountInfo) =>
            {
                System.Console.WriteLine(loginStatus.ToString());
                System.Console.WriteLine($"loginType:{accountInfo?.GetLoginType()}");
                System.Console.WriteLine($"displayName:{accountInfo?.GetDisplayName()}");

                _loginCBStatus = loginStatus;

                LoginStatus = loginStatus.ToString();

                if (loginStatus == LOGINSTATUS.LOGIN_SUCCESS)
                {

                }

            });

            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLogout(() =>
            {
                System.Console.WriteLine("log out");
            });
        }

    }
}
