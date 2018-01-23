﻿using CloudClassroom.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
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
            IsCoursesCardSelected = true;
        }

        private bool _isCoursesCardSelected;

        public bool IsCoursesCardSelected
        {
            get { return _isCoursesCardSelected; }
            set
            {
                SetProperty(ref _isCoursesCardSelected, value);
                if (value)
                {
                    SelectedCardIndex = 0;
                }
            }
        }

        private bool _isMyCardSelected;

        public bool IsMyCardSelected
        {
            get { return _isMyCardSelected; }
            set
            {
                SetProperty(ref _isMyCardSelected, value);
                if (value)
                {
                    SelectedCardIndex = 1;
                }
            }
        }

        private int _selectedCardIndex;

        public int SelectedCardIndex
        {
            get { return _selectedCardIndex; }
            set { SetProperty(ref _selectedCardIndex, value); }
        }


        public ObservableCollection<CourseModel> CourseList { get; set; }

        public ICommand JoinCommand { get; set; }
        public ICommand SelectCoursesCardCommand { get; set; }
        public ICommand SelectMyCardCommand { get; set; }



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
                    SDKError joinError = CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Start(new StartParam()
                    {
                        userType = SDKUserType.SDK_UT_APIUSER,
                        apiuserStart = new StartParam4APIUser()
                        {
                            hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                            isDirectShareDesktop = false,
                            meetingNumber = 906330844,
                            participantId = string.Empty,
                            userID = "738107",
                            userName = "我是主持人",
                            userToken = "AN5TGZgmNQc2BxQr1b0nqhAM990yFJfGfBmp",
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

            SelectCoursesCardCommand = new DelegateCommand(() =>
            {
                IsMyCardSelected = false;
                IsCoursesCardSelected = true;
            });

            SelectMyCardCommand = new DelegateCommand(() =>
            {
                IsCoursesCardSelected = false;
                IsMyCardSelected = true;
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
