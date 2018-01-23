using CloudClassroom.CustomizedUI;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MeetingViewModel:BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;
        private IntPtr _meetingUiWnd = IntPtr.Zero;

        public MeetingViewModel()
        {
            InitData();
            Task.Run(() =>
            {
                _meetingUiWnd = GetMeetingUiWnd();

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Win32APIs.SetWindowLong(_meetingUiWnd, -16, 369164288);
                    Win32APIs.SetParent(_meetingUiWnd, App.MeetingViewHwnd);
                }));
            });
        }

        private void InitData()
        {
            WindowSizeChangedCommand = new DelegateCommand(() =>
            {

            });


            WindowLocationChangedCommand = new DelegateCommand(() =>
            {

            });
        }

        private IntPtr GetMeetingUiWnd()
        {
            HWNDDotNet first = new HWNDDotNet() { value = 0 };
            HWNDDotNet second = new HWNDDotNet() { value = 0 };

            while (_sdk.GetMeetingUIWnd(ref first, ref second) != SDKError.SDKERR_SUCCESS || first.value == 0)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ms"));
            }

            return new IntPtr(first.value);
        }
        
        public ICommand WindowSizeChangedCommand { get; set; }
        public ICommand WindowLocationChangedCommand { get; set; }

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
