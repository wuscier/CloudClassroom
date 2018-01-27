using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.Models;
using CloudClassroom.sdk_adapter;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MeetingViewModel : BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;
        private bool _handledFirstMsg = false;

        public MeetingViewModel()
        {
            RegisterCallbacks();
            InitData();
        }

        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetUIHookControllerWrap().Add_CB_onUIActionNotify((type, msg) =>
            {
                Console.WriteLine($"type={type},msg={msg}");

                if (!_handledFirstMsg && type == UIHOOKHWNDTYPE.UIHOOKWNDTYPE_MAINWND && msg == 24)
                {
                    _handledFirstMsg = true;

                    HWNDDotNet first = new HWNDDotNet() { value = 0 };
                    HWNDDotNet second = new HWNDDotNet() { value = 0 };
                    _sdk.GetMeetingUIWnd(ref first, ref second);

                    App.VideoHwnd = new IntPtr(first.value);

                    Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);
                    Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                    EventAggregatorManager.Instance.EventAggregator.GetEvent<SetVideoPositionEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MeetingView,
                    });
                }
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Add_CB_onMeetingStatusChanged((status, result) =>
            {
                if (status == MeetingStatus.MEETING_STATUS_INMEETING)
                {
                    IUserInfoDotNetWrap self = _sdk.GetUserByUserID(0);
                    if (self != null)
                    {
                        App.CurrentUser.InMeetingUserId = self.GetUserID();
                        UiStatusModel.IsHost = self.IsHost();

                        string email2 = self.GetEmail();
                        uint selfId2 = self.GetUserID();
                        string selfName2 = self.GetUserNameW();
                        UserRole role2 = self.GetUserRole();
                        bool isHost2 = self.IsHost();
                    }

                    IMeetingInfo meetingInfo = _sdk.GetMeetingInfo();

                    string meetingId = meetingInfo.GetMeetingID();
                    UiStatusModel.MeetingNumber = meetingInfo.GetMeetingNumber();
                    MeetingType meetingType = meetingInfo.GetMeetingType();
                    string topic = meetingInfo.GetMeetingTopic();
                    string hostTag = meetingInfo.GetMeetingHostTag();
                    string url = meetingInfo.GetJoinMeetingUrl();


                    EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MeetingView,
                    });
                }
            });

            CMeetingParticipantsControllerDotNetWrap.Instance.Add_CB_onHostChangeNotification((hostUserId) =>
            {
                UiStatusModel.IsHost = hostUserId == App.CurrentUser.InMeetingUserId;
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingVideoController().Add_CB_onUserVideoStatusChange((userId, status) =>
            {

               // Console.WriteLine($"userId:{userId}");
            });


            CMeetingRecordingControllerDotNetWrap.Instance.Add_CB_onRecordingStatus((status) =>
            {
                Console.WriteLine($"recording status:{status}");
            });


            CMeetingParticipantsControllerDotNetWrap.Instance.Add_CB_onUserNameChanged((userId, userName) =>
            {
                Console.WriteLine($"user {userId} changed name as {userName}");
            });

            CMeetingParticipantsControllerDotNetWrap.Instance.Add_CB_onUserJoin((userIds) =>
            {
                if (userIds != null && userIds.Length > 0)
                {
                    foreach (var userId in userIds)
                    {
                        Console.WriteLine($"user join：{userId}");
                    }
                }
            });

            CMeetingParticipantsControllerDotNetWrap.Instance.Add_CB_onUserLeft((userIds) =>
            {
                if (userIds != null && userIds.Length > 0)
                {
                    foreach (var userId in userIds)
                    {
                        Console.WriteLine($"user left：{userId}");
                    }
                }
            });

            CMeetingAudioControllerDotNetWrap.Instance.Add_CB_onUserAudioStatusChange((userAudioStatuses) =>
            {
                if (userAudioStatuses != null && userAudioStatuses.Length > 0)
                {
                    foreach (var userAudioStatus in userAudioStatuses)
                    {
                        if (userAudioStatus.GetUserId() == App.CurrentUser.InMeetingUserId)
                        {
                            AudioType audioType = userAudioStatus.GetAudioType();

                            AudioStatus audioStatus = userAudioStatus.GetStatus();

                            switch (audioStatus)
                            {
                                case AudioStatus.Audio_None:

                                    break;
                                case AudioStatus.Audio_Muted:
                                case AudioStatus.Audio_Muted_ByHost:
                                case AudioStatus.Audio_MutedAll_ByHost:

                                    //SetMicUiOff();
                                    break;
                                case AudioStatus.Audio_UnMuted:
                                case AudioStatus.Audio_UnMuted_ByHost:
                                case AudioStatus.Audio_UnMutedAll_ByHost:

                                    //SetMicUiOn();
                                    break;
                            }
                        }
                    }
                }
            });


        }

        private void SetMicUiOn()
        {
            UiStatusModel.MicStatus = UiStatusModel.MicOnText;
            UiStatusModel.MicIcon = PackIconKind.Microphone.ToString();
        }

        private void SetMicUiOff()
        {
            UiStatusModel.MicStatus = UiStatusModel.MicOffText;
            UiStatusModel.MicIcon = PackIconKind.MicrophoneOff.ToString();
        }

        private void InitData()
        {
            UiStatusModel = new UiStatusModel()
            {
                BottomMenuTriggerVisible = true,
                BottomMenuTriggerIcon = PackIconKind.ChevronRight.ToString(),
                CameraIcon = PackIconKind.Video.ToString(),
                MicIcon = PackIconKind.Microphone.ToString(),
                CameraStatus = UiStatusModel.CameraOnText,
                MicStatus = UiStatusModel.MicOnText,
                IsRecording = false,
            };


            MicrophoneTriggerCommand = new DelegateCommand(() =>
            {
                switch (UiStatusModel.MicStatus)
                {
                    case UiStatusModel.MicOnText:

                        SDKError muteAudioErr = _sdk.MuteAudio(App.CurrentUser.InMeetingUserId, true);

                        if (muteAudioErr == SDKError.SDKERR_SUCCESS)
                        {
                            SetMicUiOff();
                        }
                        else
                        {
                            MessageBox.Show(Translator.TranslateSDKError(muteAudioErr));
                        }

                        break;
                    case UiStatusModel.MicOffText:
                        SDKError unmuteAudioErr = _sdk.UnmuteAudio(App.CurrentUser.InMeetingUserId);

                        if (unmuteAudioErr == SDKError.SDKERR_SUCCESS)
                        {
                            SetMicUiOn();
                        }
                        else
                        {
                            MessageBox.Show(Translator.TranslateSDKError(unmuteAudioErr));
                        }

                        break;
                }
            });

            CameraTriggerCommand = new DelegateCommand(() =>
            {
                switch (UiStatusModel.CameraStatus)
                {
                    case UiStatusModel.CameraOnText:

                        SDKError muteVideoErr = _sdk.MuteVideo();

                        if (muteVideoErr == SDKError.SDKERR_SUCCESS)
                        {
                            UiStatusModel.CameraStatus = UiStatusModel.CameraOffText;
                            UiStatusModel.CameraIcon = PackIconKind.CameraOff.ToString();
                        }
                        else
                        {
                            MessageBox.Show(Translator.TranslateSDKError(muteVideoErr));
                        }

                        break;
                    case UiStatusModel.CameraOffText:

                        SDKError unmuteVideoErr = _sdk.UnmuteVideo();

                        if (unmuteVideoErr == SDKError.SDKERR_SUCCESS)
                        {
                            UiStatusModel.CameraStatus = UiStatusModel.CameraOnText;
                            UiStatusModel.CameraIcon = PackIconKind.Camera.ToString();
                        }
                        else
                        {
                            MessageBox.Show(Translator.TranslateSDKError(unmuteVideoErr));
                        }

                        break;
                }
            });

            AudioSettingsOpenedCommand = new DelegateCommand(() =>
            {
                UiStatusModel.Microphones.Clear();

                foreach (var mic in _sdk.GetMicList())
                {
                    mic.SelectCommand = new DelegateCommand<DeviceModel>((micParam) =>
                    {
                        _sdk.SelectMic(micParam);
                    });
                    UiStatusModel.Microphones.Add(mic);
                }

                UiStatusModel.Speakers.Clear();

                foreach (var speaker in _sdk.GetSpeakerList())
                {
                    speaker.SelectCommand = new DelegateCommand<DeviceModel>((speakerParam) =>
                    {
                        _sdk.SelectSpeaker(speakerParam);
                    });
                    UiStatusModel.Speakers.Add(speaker);
                }

            });

            VideoSettingsOpenedCommand = new DelegateCommand(() =>
            {
                UiStatusModel.Cameras.Clear();

                foreach (var camera in _sdk.GetCameraList())
                {
                    camera.SelectCommand = new DelegateCommand<DeviceModel>((cameraParam) =>
                    {
                        _sdk.SelectCamera(cameraParam);
                    });
                    UiStatusModel.Cameras.Add(camera);
                }

            });

            RecordTriggerCommand = new DelegateCommand(() =>
            {
                if (UiStatusModel.IsRecording)
                {
                    DateTime startTime = DateTime.Now;

                    SDKError stopRecordErr = _sdk.StopRecording(ref startTime);


                    UiStatusModel.IsRecording = false;

                    //if (stopRecordErr == SDKError.SDKERR_SUCCESS)
                    //{
                    //    UiStatusModel.IsRecording = false;
                    //}
                    //else
                    //{
                    //    MessageBox.Show(Translator.TranslateSDKError(stopRecordErr));
                    //}

                }
                else
                {

                    DateTime stopTime = DateTime.Now;
                    string recordPath = _sdk.GetRecordingPath();
                    if (!Directory.Exists(recordPath))
                    {
                        MessageBox.Show($"录制路径：{recordPath}不存在！");
                        return;
                    }

                    SDKError startRecordErr = _sdk.StartRecording(ref stopTime, recordPath);

                    UiStatusModel.IsRecording = true;


                    //if (startRecordErr == SDKError.SDKERR_SUCCESS)
                    //{
                    //    UiStatusModel.IsRecording = true;
                    //}
                    //else
                    //{
                    //    MessageBox.Show(Translator.TranslateSDKError(startRecordErr));
                    //}
                }
            });

            ShowRecordPathCommand = new DelegateCommand(() =>
            {
                EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowRecordPathEvent>().Publish(new EventArgument()
                {
                    Target = Target.MeetingView,
                });
            });

            OpenShareOptionsCommand = new DelegateCommand(() =>
            {
                EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowSharingOptionsEvent>().Publish(new EventArgument()
                {
                    Target = Target.MeetingView,
                });
            });

            ShowParticipantsDialogCommand = new DelegateCommand(() =>
            {
                IUserInfoDotNetWrap self = _sdk.GetUserByUserID(0);
                string email2 = self.GetEmail();
                uint selfId2 = self.GetUserID();
                string selfName2 = self.GetUserNameW();
                UserRole role2 = self.GetUserRole();
                bool isHost2 = self.IsHost();


                uint[] users = _sdk.GetParticipantsList();

                IUserInfoDotNetWrap user = _sdk.GetUserByUserID(users[0]);

                string email = user.GetEmail();
                uint userId = user.GetUserID();
                string userName = user.GetUserNameW();
                UserRole role = user.GetUserRole();
                bool isHost = user.IsHost();

            });

            BottomMenuTriggerCommand = new DelegateCommand(() =>
            {
                if (UiStatusModel.BottomMenuVisible)
                {
                    UiStatusModel.BottomMenuTriggerIcon = PackIconKind.ChevronRight.ToString();
                }
                else
                {
                    UiStatusModel.BottomMenuTriggerIcon = PackIconKind.ChevronLeft.ToString();
                }

                UiStatusModel.BottomMenuVisible = !UiStatusModel.BottomMenuVisible;
            });

            WindowStateChangedCommand = new DelegateCommand(() =>
            {
                //switch (state)
                //{
                //    case WindowState.Maximized:
                //    case WindowState.Normal:
                //        UiStatusModel.BottomMenuTriggerVisible = true;
                //        break;
                //    case WindowState.Minimized:
                //        UiStatusModel.BottomMenuVisible = false;
                //        UiStatusModel.BottomMenuTriggerVisible = false;
                //        break;
                //}
            });
        }

        public UiStatusModel UiStatusModel { get; set; }

        public ICommand MicrophoneTriggerCommand { get; set; }
        public ICommand AudioSettingsOpenedCommand { get; set; }
        public ICommand CameraTriggerCommand { get; set; }
        public ICommand VideoSettingsOpenedCommand { get; set; }

        public ICommand ShowParticipantsDialogCommand { get; set; }
        public ICommand OpenShareOptionsCommand { get; set; }
        public ICommand RecordTriggerCommand { get; set; }
        public ICommand ShowRecordPathCommand { get; set; }

        public ICommand BottomMenuTriggerCommand { get; set; }
        public ICommand WindowStateChangedCommand { get; set; }
    }



    public class UiStatusModel : BindableBase
    {
        public UiStatusModel()
        {
            Microphones = new ObservableCollection<DeviceModel>();
            Speakers = new ObservableCollection<DeviceModel>();
            Cameras = new ObservableCollection<DeviceModel>();
        }



        public const string MicOnText = "静音";
        public const string MicOffText = "解除静音";

        public const string CameraOnText = "停止视频";
        public const string CameraOffText = "启动视频";

        private ulong _meetingNumber;

        public ulong MeetingNumber
        {
            get { return _meetingNumber; }
            set { SetProperty(ref _meetingNumber, value); }
        }

        private bool _isHost;
        public bool IsHost
        {
            get { return _isHost; }
            set { SetProperty(ref _isHost, value); }
        }

        private string _micStatus;
        public string MicStatus
        {
            get { return _micStatus; }
            set { SetProperty(ref _micStatus, value); }
        }

        private string _micIcon;
        public string MicIcon
        {
            get { return _micIcon; }
            set { SetProperty(ref _micIcon, value); }
        }

        private string _cameraStatus;
        public string CameraStatus
        {
            get { return _cameraStatus; }
            set { SetProperty(ref _cameraStatus, value); }
        }

        private string _cameraIcon;
        public string CameraIcon
        {
            get { return _cameraIcon; }
            set { SetProperty(ref _cameraIcon, value); }
        }

        private bool _isRecording;
        public bool IsRecording
        {
            get { return _isRecording; }
            set { SetProperty(ref _isRecording, value); }
        }

        private string _bottomMenuTriggerIcon;
        public string BottomMenuTriggerIcon
        {
            get { return _bottomMenuTriggerIcon; }
            set { SetProperty(ref _bottomMenuTriggerIcon, value); }
        }

        private bool _bottomMenuTriggerVisible;
        public bool BottomMenuTriggerVisible
        {
            get { return _bottomMenuTriggerVisible; }
            set { SetProperty(ref _bottomMenuTriggerVisible, value); }
        }

        private bool _bottomMenuVisible;
        public bool BottomMenuVisible
        {
            get { return _bottomMenuVisible; }
            set { SetProperty(ref _bottomMenuVisible, value); }
        }

        public ObservableCollection<DeviceModel> Microphones { get; set; }
        public ObservableCollection<DeviceModel> Speakers { get; set; }
        public ObservableCollection<DeviceModel> Cameras { get; set; }
    }



}
