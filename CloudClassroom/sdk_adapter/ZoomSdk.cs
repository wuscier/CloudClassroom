using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public SDKError Leave(LeaveMeetingCmd leaveMeetingCmd)
        {
            throw new NotImplementedException();
        }

        public SDKError Login(LoginParam loginParam)
        {
            throw new NotImplementedException();
        }

        public SDKError Logout()
        {
            throw new NotImplementedException();
        }

        public SDKError MuteAudio(uint userId, bool allowUnmuteBySelf)
        {
            throw new NotImplementedException();
        }

        public SDKError MuteVideo()
        {
            throw new NotImplementedException();
        }

        public SDKError SDKAuth(AuthParam authParam)
        {
            return CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().SDKAuth(authParam);
        }

        public SDKError Start(StartParam startParam)
        {
            throw new NotImplementedException();
        }

        public SDKError StartAppShare(HWNDDotNet hWNDDotNet)
        {
            throw new NotImplementedException();
        }

        public SDKError StartMonitorShare()
        {
            throw new NotImplementedException();
        }

        public SDKError StartRecording(DateTime startTimestamp, string recPath)
        {
            throw new NotImplementedException();
        }

        public SDKError StopRecording(DateTime stopTimestamp)
        {
            throw new NotImplementedException();
        }

        public SDKError StopShare()
        {
            throw new NotImplementedException();
        }

        public SDKError UnmuteAudio(uint userId)
        {
            throw new NotImplementedException();
        }

        public SDKError UnmuteVideo()
        {
            throw new NotImplementedException();
        }
    }
}
