using System;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.sdk_adapter
{
    public interface ISdk
    {
        string MeetingUiClassName { get; }
        string BottomToolbarClassName { get; }

        SDKError Initialize(InitParam initParam);
        SDKError CleanUp();

        SDKError SDKAuth(AuthParam authParam);
        SDKError Login(LoginParam loginParam);
        SDKError Logout();

        SDKError Join(JoinParam joinParam);
        SDKError Start(StartParam startParam);
        SDKError Leave(LeaveMeetingCmd leaveMeetingCmd);

        SDKError StartRecording(DateTime startTimestamp, string recPath);
        SDKError StopRecording(DateTime stopTimestamp);

        SDKError StartAppShare(HWNDDotNet hWNDDotNet);
        SDKError StartMonitorShare();
        SDKError StopShare();

        SDKError MuteVideo();
        SDKError UnmuteVideo();
        SDKError MuteAudio(uint userId, bool allowUnmuteBySelf);
        SDKError UnmuteAudio(uint userId);

        SDKError GetMeetingUIWnd(ref HWNDDotNet first, ref HWNDDotNet second);


        SDKError MonitorWndMessage(uint id, bool add);
        SDKError MonitorWnd(string name, bool add);
        SDKError StartMonitor();
        SDKError StopMonitor();


    }
}
