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
                    Target = Target.MainView,
                });
            });

            JoinCommand = new DelegateCommand<LessonModel>((lesson) =>
            {
                ulong uint_meeting_number;
                if (!ulong.TryParse(lesson.MeetingId, out uint_meeting_number))
                {
                    MessageBox.Show("无效的课堂号！");
                    return;
                };

                if (lesson.SpeakUserId == App.CurrentUser.Id)
                {
                    SDKError startError = _sdk.Start(new StartParam()
                    {
                        userType = SDKUserType.SDK_UT_APIUSER,
                        apiuserStart = new StartParam4APIUser()
                        {
                            hDirectShareAppWnd = new HWNDDotNet() { value = 0 },
                            isDirectShareDesktop = false,
                            meetingNumber = 856848506,
                            participantId = string.Empty,
                            userID = "LaWc3yx0RwCz2SFZVFhDaQ",
                            userName = "主持人",
                            userToken = App.ZoomInfo.AccessToken,
                        },
                    });

                    if (startError == SDKError.SDKERR_SUCCESS)
                    {
                        EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                        {
                            Target = Target.MainView
                        });

                    }
                    else
                    {
                        MessageBox.Show(SdkErrorTranslator.TranslateSDKError(startError));
                    }
                }
                else
                {
                    SDKError joinError = _sdk.Join(new JoinParam()
                    {
                        userType = SDKUserType.SDK_UT_APIUSER,
                        apiuserJoin = new JoinParam4APIUser()
                        {
                            userName = "非主持人",
                            meetingNumber = 856848506,
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
                        EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Publish(new EventArgument()
                        {
                            Target = Target.MainView
                        });

                    }
                    else
                    {
                        MessageBox.Show(SdkErrorTranslator.TranslateSDKError(joinError));
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

            LoadLessonsCommand = new DelegateCommand(async () =>
            {
                IList<LessonModel> lessons = await WebApi.Instance.GetWeeklyLessons();

                foreach (var lesson in lessons)
                {
                    lesson.DetailCommand = DetailCommand;
                    CourseList.Add(lesson);
                }

            });

        }
    }
}
