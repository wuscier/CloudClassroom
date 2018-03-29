using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.Models;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Service;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MainViewModel:BindableBase
    {

        private ISdk _sdk = ZoomSdk.Instance;

        public MainViewModel()
        {
            InitData();
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

        public ObservableCollection<LessonModel> CourseList { get; set; }

        public ICommand DetailCommand { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand SelectCoursesCardCommand { get; set; }
        public ICommand SelectMyCardCommand { get; set; }
        public ICommand LoadLessonsCommand { get; set; }

        private void InitData()
        {
            CourseList = new ObservableCollection<LessonModel>();

            IsCoursesCardSelected = true;

            DetailCommand = new DelegateCommand<LessonModel>((lesson) =>
            {
                EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowLessonDetailEvent>().Publish(new EventArgument()
                {
                    Argument = new Argument()
                    {
                        Value = lesson,
                    },
                    Target = Target.MainView,
                });
            });

            JoinCommand = new DelegateCommand<LessonModel>(async(lesson) =>
            {
                SDKError joinError = _sdk.Join(new JoinParam()
                {
                    userType = SDKUserType.SDK_UT_APIUSER,
                    apiuserJoin = new JoinParam4APIUser()
                    {
                        userName = App.CurrentUser.Name,
                        meetingNumber = ulong.Parse(lesson.MeetingId),
                        //psw = zoomMeeting.Password,
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
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MainView
                    });

                }
                else
                {
                    MessageBox.Show(SdkErrorTranslator.TranslateSDKError(joinError));
                }






                //ZoomMeetingModel zoomMeeting = await WebApi.Instance.GetMeetingInfo(lesson.MeetingId);

                //if (zoomMeeting == null)
                //{
                //    MessageBox.Show("获取课堂信息失败！");
                //    return;
                //}

                //ulong meetingNumber;
                //if (!ulong.TryParse(zoomMeeting.MeetingId, out meetingNumber))
                //{
                //    MessageBox.Show("无效的课堂号！");
                //    return;
                //}

                //if (App.CurrentUser.Id == lesson.SpeakUserId)
                //{
                //    SDKError startError = _sdk.Start(new StartParam()
                //    {
                //        userType = SDKUserType.SDK_UT_APIUSER,
                //        apiuserStart = new StartParam4APIUser()
                //        {
                //            hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                //            isDirectShareDesktop = false,
                //            meetingNumber = meetingNumber,
                //            participantId = string.Empty,
                //            userID = zoomMeeting.HostId,
                //            userName = App.CurrentUser.Name,
                //            userToken = App.ZoomInfo.AccessToken,
                //        },
                //    });

                //    if (startError == SDKError.SDKERR_SUCCESS)
                //    {
                //        EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                //        {
                //            Target = Target.MainView
                //        });

                //    }
                //    else
                //    {
                //        MessageBox.Show(SdkErrorTranslator.TranslateSDKError(startError));
                //    }
                //}
                //else
                //{
                //    SDKError joinError = _sdk.Join(new JoinParam()
                //    {
                //        userType = SDKUserType.SDK_UT_APIUSER,
                //        apiuserJoin = new JoinParam4APIUser()
                //        {
                //            userName = App.CurrentUser.Name,
                //            meetingNumber = meetingNumber,
                //            psw = zoomMeeting.Password,
                //            hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                //            isAudioOff = false,
                //            isDirectShareDesktop = false,
                //            isVideoOff = false,
                //            participantId = string.Empty,
                //            toke4enfrocelogin = string.Empty,
                //            webinarToken = string.Empty,
                //        }
                //    });

                //    if (joinError == SDKError.SDKERR_SUCCESS)
                //    {
                //        EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                //        {
                //            Target = Target.MainView
                //        });

                //    }
                //    else
                //    {
                //        MessageBox.Show(SdkErrorTranslator.TranslateSDKError(joinError));
                //    }
                //}
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

            LoadLessonsCommand = new DelegateCommand(async () =>
            {
                IList<LessonModel> lessons = await WebApi.Instance.GetWeeklyLessons();

                foreach (var lesson in lessons)
                {
                    lesson.DetailCommand = DetailCommand;
                    lesson.JoinCommand = JoinCommand;
                    CourseList.Add(lesson);
                }




                CourseList.Add(new LessonModel()
                {
                    CooperationType =0,
                    DetailCommand = DetailCommand,
                    JoinCommand = JoinCommand,
                    EndTime = "2018-03-29 11:00:00",
                    MeetingId = "5934861413",
                    LessonType =0,
                    Name = "test0329",
                    StartTime = "2018-03-29 10:30:00",
                    SpeakUserId = "2899989669@qq.com",
                    
                });

            });

        }
    }
}
