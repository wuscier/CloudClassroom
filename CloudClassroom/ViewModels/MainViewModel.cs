using CloudClassroom.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MainViewModel:BindableBase
    {
        public MainViewModel()
        {
            InitData();
            IsMainCardSelected = true;
        }

        private bool _isMainCardSelected;

        public bool IsMainCardSelected
        {
            get { return _isMainCardSelected; }
            set { SetProperty(ref _isMainCardSelected, value); }
        }

        private bool _isHistoryCardSelected;

        public bool IsHistoryCardSelected
        {
            get { return _isHistoryCardSelected; }
            set { SetProperty(ref _isHistoryCardSelected, value); }
        }

        public ObservableCollection<CourseModel> CourseList { get; set; }

        public ICommand JoinCommand { get; set; }


        private void InitData()
        {
            JoinCommand = new DelegateCommand<CourseModel>((course) =>
            {
                ulong uint_meeting_number;
                if (!ulong.TryParse(course.MeetingNumber, out uint_meeting_number))
                {
                    MessageBox.Show("无效的课堂号！");
                    return;
                };

                if (course.HostId == App.UserModel.UserName)
                {
                    CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onLoginRet((loginStatus, accountInfo) =>
                    {
                        switch (loginStatus)
                        {
                            case LOGINSTATUS.LOGIN_IDLE:
                                break;
                            case LOGINSTATUS.LOGIN_PROCESSING:
                                break;
                            case LOGINSTATUS.LOGIN_SUCCESS:

                                SDKError joinError = CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(new JoinParam()
                                {
                                    userType = SDKUserType.SDK_UT_NORMALUSER,
                                    normaluserJoin = new JoinParam4NormalUser()
                                    {
                                        hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                                        isAudioOff = false,
                                        isDirectShareDesktop = false,
                                        isVideoOff = false,
                                        meetingNumber = uint_meeting_number,
                                        participantId = string.Empty,
                                        psw = string.Empty,
                                        userName = "主持人",
                                        webinarToken = string.Empty,
                                    }
                                });

                                if (joinError == SDKError.SDKERR_SUCCESS)
                                {
                                    MessageBox.Show("加入课堂成功！");
                                }
                                else
                                {
                                    MessageBox.Show(joinError.ToString());
                                }



                                break;
                            case LOGINSTATUS.LOGIN_FAILED:
                                break;
                            default:
                                break;
                        }
                    });

                    SDKError loginError = CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Login(new LoginParam()
                    {
                        loginType = LoginType.LoginType_Email,
                        emailLogin = new LoginParam4Email()
                        {
                            bRememberMe = true,
                            password = "justlucky",
                            userName = course.HostId,
                        }
                    });
                }
                else
                {
                    SDKError joinError = CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(new JoinParam()
                    {
                        userType = SDKUserType.SDK_UT_APIUSER,
                        apiuserJoin = new JoinParam4APIUser()
                        {
                            userName = "我是API用户",
                            meetingNumber = uint_meeting_number,
                            psw = string.Empty,
                            hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                            isAudioOff = false,
                            isDirectShareDesktop = false,
                            isVideoOff = false,
                            participantId = string.Empty,
                            toke4enfrocelogin = string.Empty,
                            webinarToken = string.Empty,
                        }
                    });

                    if (joinError == SDKError.SDKERR_SUCCESS)
                    {
                        MessageBox.Show("加入课堂成功！");
                    }
                    else
                    {
                        MessageBox.Show(joinError.ToString());
                    }
                }
            });

            CourseList = new ObservableCollection<CourseModel>();
            CourseList.Add(new CourseModel()
            {
                Duration = "8:00 - 9:00",
                Name = "语文",
                TeacherName = "马云",
                MeetingNumber = "286683782",
                HostId = "justlucky@126.com",
                JoinCommand = JoinCommand,
            });
            CourseList.Add(new CourseModel()
            {
                Duration = "11:00 - 12:00",
                Name = "数学",
                TeacherName = "刘强东",
                MeetingNumber = "286683782",
                HostId = "justlucky@126.com",
                JoinCommand = JoinCommand,

            });
            CourseList.Add(new CourseModel()
            {
                Duration = "14:00 - 15:00",
                Name = "生物",
                TeacherName = "李海波",
                MeetingNumber = "286683782",
                HostId = "justlucky@126.com",
                JoinCommand = JoinCommand,

            });
        }

    }
}
