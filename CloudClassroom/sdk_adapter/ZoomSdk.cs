using System;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.sdk_adapter
{
    public class ZoomSdk : ISdk
    {

        public static readonly ZoomSdk Instance = new ZoomSdk();

        public SDKError CleanUp()
        {
            return CZoomSDKeDotNetWrap.Instance.CleanUp();
        }

        public SDKError GetMeetingUIWnd(ref HWNDDotNet first, ref HWNDDotNet second)
        {
            throw new NotImplementedException();
        }

        public SDKError Initialize(InitParam initParam)
        {
            return CZoomSDKeDotNetWrap.Instance.Initialize(initParam);
        }

        public SDKError Join(JoinParam joinParam)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Join(joinParam);
        }

        public SDKError Leave(LeaveMeetingCmd leaveMeetingCmd)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Leave(leaveMeetingCmd);
        }

        public SDKError Login(LoginParam loginParam)
        {
            return CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Login(loginParam);
        }

        public SDKError Logout()
        {
            return CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().LogOut();
        }

        public SDKError MuteAudio(uint userId, bool allowUnmuteBySelf)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingAudioController().MuteAudio(userId, allowUnmuteBySelf);
        }

        public SDKError MuteVideo()
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingVideoController().MuteVideo();
        }

        public SDKError SDKAuth(AuthParam authParam)
        {
            return CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().SDKAuth(authParam);
        }

        public SDKError Start(StartParam startParam)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().Start(startParam);
        }

        public SDKError StartAppShare(HWNDDotNet hWNDDotNet)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StartAppShare(hWNDDotNet);
        }

        public SDKError StartMonitorShare()
        {
            throw new NotImplementedException();
        }

        public SDKError StartRecording(DateTime startTimestamp, string recPath)
        {
            ValueType valueType = startTimestamp;
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingRecordingController().StartRecording(ref valueType, recPath);
        }

        public SDKError StopRecording(DateTime stopTimestamp)
        {
            ValueType valueType = stopTimestamp;
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingRecordingController().StopRecording(ref valueType);
        }

        public SDKError StopShare()
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingShareController().StopShare();
        }

        public SDKError UnmuteAudio(uint userId)
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingAudioController().UnMuteAudio(userId);
        }

        public SDKError UnmuteVideo()
        {
            return CZoomSDKeDotNetWrap.Instance.GetMeetingServiceWrap().GetMeetingVideoController().UnmuteVideo();
        }
    }
}
