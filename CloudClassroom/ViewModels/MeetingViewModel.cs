﻿using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.Models;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MeetingViewModel:BindableBase
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

                if (!_handledFirstMsg && type == UIHOOKHWNDTYPE.UIHOOKWNDTYPE_MAINWND)
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
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<IntoMeetingSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MeetingView,
                    });
                }
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingVideoController().Add_CB_onUserVideoStatusChange((userId, status) =>
            {
                if (status == VideoStatus.Video_ON)
                {
                }
            });


        }

        private void InitData()
        {
            MicrophoneTriggerCommand = new DelegateCommand(() =>
            {
                //_sdk.StartMonitor();
            });

            CameraTriggerCommand = new DelegateCommand(() =>
            {
                //_sdk.StopMonitor();
            });
        }

        public UiStatusModel UiStatusModel { get; set; }

        public ICommand MicrophoneTriggerCommand { get; set; }
        public ICommand AudioSettingsOpenedCommand { get; set; }
        public ICommand AudioSelectedCommand { get; set; }
        public ICommand SpeakerSelectedCommand { get; set; }
        public ICommand CameraTriggerCommand { get; set; }
        public ICommand VideoSettingsOpenedCommand { get; set; }
        public ICommand VideoSelectedCommand { get; set; }
        public ICommand OpenShareOptionsCommand { get; set; }
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

        public const string RecordPauseText = "暂停录制";
        public const string RecordResumeText = "恢复录制";

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

        private string _pauseResumeKind;

        public string PauseResumeKind
        {
            get { return _pauseResumeKind; }
            set { SetProperty(ref _pauseResumeKind, value); }
        }


        private string _pauseResumeText;

        public string PauseResumeText
        {
            get { return _pauseResumeText; }
            set { SetProperty(ref _pauseResumeText, value); }
        }


        public ObservableCollection<DeviceModel> Microphones { get; set; }
        public ObservableCollection<DeviceModel> Speakers { get; set; }
        public ObservableCollection<DeviceModel> Cameras { get; set; }
    }



}
