using CloudClassroom.CustomizedUI;
using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
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

        public MeetingViewModel()
        {
            RegisterCallbacks();
            InitData();
        }


        private bool _handledFirstMsg = false;

        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetUIHookControllerWrap().Add_CB_onUIActionNotify((type, msg) =>
            {
                Console.WriteLine($"type={type},msg={msg}");

                if (type == UIHOOKHWNDTYPE.UIHOOKWNDTYPE_MAINWND && !_handledFirstMsg)
                {
                    _handledFirstMsg = true;

                    HWNDDotNet first = new HWNDDotNet() { value = 0 };
                    HWNDDotNet second = new HWNDDotNet() { value = 0 };
                    _sdk.GetMeetingUIWnd(ref first, ref second);

                    App.VideoHwnd = new IntPtr(first.value);

                    Win32APIs.SetWindowLong(App.VideoHwnd, -16, 369164288);
                    Win32APIs.SetParent(App.VideoHwnd, App.MeetingViewHwnd);

                    EventAggregatorManager.Instance.EventAggregator.GetEvent<VideoUiAdaptedEvent>().Publish(new EventArgument()
                    {
                        Target = Target.MeetingView,
                    });
                }
            });

            CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Add_CB_onMeetingStatusChanged((status, result) =>
            {
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

        public ICommand MicrophoneTriggerCommand { get; set; }
        public ICommand AudioSettingsOpenedCommand { get; set; }
        public ICommand AudioSelectedCommand { get; set; }
        public ICommand SpeakerSelectedCommand { get; set; }
        public ICommand CameraTriggerCommand { get; set; }
        public ICommand VideoSettingsOpenedCommand { get; set; }
        public ICommand VideoSelectedCommand { get; set; }
        public ICommand OpenShareOptionsCommand { get; set; }
    }
}
