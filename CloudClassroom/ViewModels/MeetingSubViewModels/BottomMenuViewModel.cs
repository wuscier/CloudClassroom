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
    public class BottomMenuViewModel : BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;

        public BottomMenuViewModel()
        {
            RegisterCallbacks();
            InitData();
        }

        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Add_CB_onMeetingStatusChanged((status, result) =>
            {
                Console.WriteLine($"onMeetingStatusChanged  status:{status}, result:{result}");

                if (status == MeetingStatus.MEETING_STATUS_INMEETING)
                {
                    IUserInfoDotNetWrap self = _sdk.GetUserByUserID(0);
                    if (self != null)
                    {
                        App.CurrentUser.InMeetingUserId = self.GetUserID();
                        IsHost = self.IsHost();

                        string email2 = self.GetEmail();
                        uint selfId2 = self.GetUserID();
                        string selfName2 = self.GetUserNameW();
                        UserRole role2 = self.GetUserRole();
                        bool isHost2 = self.IsHost();
                    }

                    IMeetingInfo meetingInfo = _sdk.GetMeetingInfo();

                    string meetingId = meetingInfo.GetMeetingID();
                    MeetingNumber = meetingInfo.GetMeetingNumber();
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

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onHostChangeNotification((hostUserId) =>
            {
                Console.WriteLine($"onHostChangeNotification    hostUserId:{hostUserId}");
                IsHost = hostUserId == App.CurrentUser.InMeetingUserId;
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingVideoController().Add_CB_onUserVideoStatusChange((userId, status) =>
            {
                Console.WriteLine($"onUserVideoStatusChange userId:{userId}, status:{status}");
                // Console.WriteLine($"userId:{userId}");
            });


            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingRecordingController().Add_CB_onRecordingStatus((status) =>
            {
                Console.WriteLine($"onRecordingStatus   status:{status}");
            });


            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserNameChanged((userId, userName) =>
            {
                Console.WriteLine($"onUserNameChanged   userId:{userId}, userName:{userName}");
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserJoin((userIds) =>
            {
                if (userIds != null && userIds.Length > 0)
                {
                    foreach (var userId in userIds)
                    {
                        Console.WriteLine($"onUserJoin  userId:{userId}");
                    }
                }
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingParticipantsController().Add_CB_onUserLeft((userIds) =>
            {
                if (userIds != null && userIds.Length > 0)
                {
                    foreach (var userId in userIds)
                    {
                        Console.WriteLine($"onUserLeft  userId:{userId}");
                    }
                }
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingAudioController().Add_CB_onUserAudioStatusChange((userAudioStatuses) =>
            {
                if (userAudioStatuses != null && userAudioStatuses.Length > 0)
                {
                    foreach (var userAudioStatus in userAudioStatuses)
                    {
                        Console.WriteLine($"onUserAudioStatusChange status:{userAudioStatuses}");

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

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().Add_CB_onShareContentNotification((shareInfo) =>
            {
                ShareInfo shareInformation = (ShareInfo)shareInfo;

                Console.WriteLine($"onShareContentNotification  type:{shareInformation.eShareType}, hwnd:{shareInformation.hwndSharedApp}, monitorId:{shareInformation.monitorID}");
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().Add_CB_onSharingStatus((status,userId) =>
            {
                Console.WriteLine($"onSharingStatus status：{status}, userId：{userId}");

                switch (status)
                {
                    case SharingStatus.Sharing_Self_Send_Begin:
                        EventAggregatorManager.Instance.EventAggregator.GetEvent<HideMeetingViewEvent>().Publish(new EventArgument()
                        {
                            Target = Target.MeetingView,
                        });
                        break;
                    case SharingStatus.Sharing_Self_Send_End:
                        EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowMeetingViewEvent>().Publish(new EventArgument()
                        {
                            Target = Target.MeetingView,
                        });
                        break;
                    case SharingStatus.Sharing_Other_Share_Begin:
                        break;
                    case SharingStatus.Sharing_Other_Share_End:
                        EventAggregatorManager.Instance.EventAggregator.GetEvent<ResetVideoUiEvent>().Publish(new EventArgument()
                        {
                            Target = Target.MeetingView,
                        });
                        break;
                    case SharingStatus.Sharing_View_Other_Sharing:
                        break;
                    case SharingStatus.Sharing_Pause:
                        break;
                    case SharingStatus.Sharing_Resume:
                        break;
                    default:
                        break;
                }
            });

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

        public ObservableCollection<DeviceModel> Microphones { get; set; }
        public ObservableCollection<DeviceModel> Speakers { get; set; }
        public ObservableCollection<DeviceModel> Cameras { get; set; }

        public ICommand MicrophoneTriggerCommand { get; set; }
        public ICommand AudioSettingsOpenedCommand { get; set; }
        public ICommand CameraTriggerCommand { get; set; }
        public ICommand VideoSettingsOpenedCommand { get; set; }

        public ICommand ShowParticipantsDialogCommand { get; set; }
        public ICommand OpenShareOptionsCommand { get; set; }
        public ICommand RecordTriggerCommand { get; set; }
        public ICommand ShowRecordPathCommand { get; set; }

        private void SetMicUiOn()
        {
            MicStatus = MicOnText;
            MicIcon = PackIconKind.Microphone.ToString();
        }

        private void SetMicUiOff()
        {
            MicStatus = MicOffText;
            MicIcon = PackIconKind.MicrophoneOff.ToString();
        }

        private void InitData()
        {
            Microphones = new ObservableCollection<DeviceModel>();
            Speakers = new ObservableCollection<DeviceModel>();
            Cameras = new ObservableCollection<DeviceModel>();

                CameraIcon = PackIconKind.Video.ToString();
                MicIcon = PackIconKind.Microphone.ToString();
                CameraStatus = CameraOnText;
                MicStatus = MicOnText;
                IsRecording = false;


            MicrophoneTriggerCommand = new DelegateCommand(() =>
            {
                switch (MicStatus)
                {
                    case MicOnText:

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
                    case MicOffText:
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
                switch (CameraStatus)
                {
                    case CameraOnText:

                        SDKError muteVideoErr = _sdk.MuteVideo();

                        if (muteVideoErr == SDKError.SDKERR_SUCCESS)
                        {
                            CameraStatus = CameraOffText;
                            CameraIcon = PackIconKind.CameraOff.ToString();
                        }
                        else
                        {
                            MessageBox.Show(Translator.TranslateSDKError(muteVideoErr));
                        }

                        break;
                    case CameraOffText:

                        SDKError unmuteVideoErr = _sdk.UnmuteVideo();

                        if (unmuteVideoErr == SDKError.SDKERR_SUCCESS)
                        {
                            CameraStatus = CameraOnText;
                            CameraIcon = PackIconKind.Camera.ToString();
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
                Microphones.Clear();

                foreach (var mic in _sdk.GetMicList())
                {
                    mic.SelectCommand = new DelegateCommand<DeviceModel>((micParam) =>
                    {
                        _sdk.SelectMic(micParam);
                    });
                    Microphones.Add(mic);
                }

                Speakers.Clear();

                foreach (var speaker in _sdk.GetSpeakerList())
                {
                    speaker.SelectCommand = new DelegateCommand<DeviceModel>((speakerParam) =>
                    {
                        _sdk.SelectSpeaker(speakerParam);
                    });
                    Speakers.Add(speaker);
                }

            });

            VideoSettingsOpenedCommand = new DelegateCommand(() =>
            {
                Cameras.Clear();

                foreach (var camera in _sdk.GetCameraList())
                {
                    camera.SelectCommand = new DelegateCommand<DeviceModel>((cameraParam) =>
                    {
                        _sdk.SelectCamera(cameraParam);
                    });
                    Cameras.Add(camera);
                }

            });

            RecordTriggerCommand = new DelegateCommand(() =>
            {
                if (IsRecording)
                {
                    DateTime startTime = DateTime.Now;

                    SDKError stopRecordErr = _sdk.StopRecording(ref startTime);


                    IsRecording = false;

                    //if (stopRecordErr == SDKError.SDKERR_SUCCESS)
                    //{
                    //    IsRecording = false;
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

                    IsRecording = true;


                    //if (startRecordErr == SDKError.SDKERR_SUCCESS)
                    //{
                    //    IsRecording = true;
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
        }
    }
}
