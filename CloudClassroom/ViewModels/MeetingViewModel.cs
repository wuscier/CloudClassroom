using CloudClassroom.Events;
using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using CloudClassroom.Views;
using Prism.Mvvm;
using System;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class MeetingViewModel : BindableBase
    {
        private ISdk _sdk = ZoomSdk.Instance;
        private bool _handledFirstMsg = false;


        private ulong _meetingNumber;

        public ulong MeetingNumber
        {
            get { return _meetingNumber; }
            set { SetProperty(ref _meetingNumber, value); }
        }

        public BottomMenuViewModel BottomMenuViewModel { get; set; }

        public MeetingViewModel()
        {
            RegisterCallbacks();

            BottomMenuView bottomMenuView = new BottomMenuView();
            BottomMenuViewModel = new BottomMenuViewModel();

            bottomMenuView.DataContext = BottomMenuViewModel;

            bottomMenuView.Show();
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
                        BottomMenuViewModel.IsHost = self.IsHost();

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

            CMeetingParticipantsControllerDotNetWrap.Instance.Add_CB_onHostChangeNotification((hostUserId) =>
            {
                BottomMenuViewModel.IsHost = hostUserId == App.CurrentUser.InMeetingUserId;
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



    }
}
