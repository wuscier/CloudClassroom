using CloudClassroom.Models;
using System;
using System.Collections.Generic;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.sdk_adapter
{
    public interface ISdk
    {
        SDKError Initialize(InitParam initParam);
        SDKError CleanUp();

        SDKError SDKAuth(AuthParam authParam);
        SDKError Login(LoginParam loginParam);
        SDKError Logout();

        void CustomizeUI();

        SDKError EnterFullScreen();
        SDKError ExitFullScreen();

        SDKError Join(JoinParam joinParam);
        SDKError Start(StartParam startParam);
        SDKError Leave(LeaveMeetingCmd leaveMeetingCmd);

        SDKError StartRecording(ref DateTime startTimestamp, string recPath);
        SDKError StopRecording(ref DateTime stopTimestamp);

        SDKError SetRecordingPath(string path);
        string GetRecordingPath();

        SDKError StartAppShare(HWNDDotNet hWNDDotNet);
        SDKError StartMonitorShare();
        SDKError StopShare();

        IList<DeviceModel> GetMicList();
        IList<DeviceModel> GetSpeakerList();
        IList<DeviceModel> GetCameraList();

        SDKError SelectMic(DeviceModel mic);
        SDKError SelectSpeaker(DeviceModel speaker);
        SDKError SelectCamera(DeviceModel camera);


        SDKError MuteVideo();
        SDKError UnmuteVideo();
        SDKError MuteAudio(uint userId, bool allowUnmuteBySelf);
        SDKError UnmuteAudio(uint userId);

        SDKError GetMeetingUIWnd(ref HWNDDotNet first, ref HWNDDotNet second);


        SDKError MonitorWndMessage(uint id, bool add);
        SDKError MonitorWnd(string name, bool add);
        SDKError StartMonitor();
        SDKError StopMonitor();

        uint[] GetParticipantsList();
        IUserInfoDotNetWrap GetUserByUserID(uint userId);

        IMeetingInfo GetMeetingInfo();
    }
}
