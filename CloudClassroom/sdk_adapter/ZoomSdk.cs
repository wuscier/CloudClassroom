﻿using System;
using System.Collections.Generic;
using CloudClassroom.Models;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.sdk_adapter
{
    public class ZoomSdk : ISdk
    {

        public static readonly ZoomSdk Instance = new ZoomSdk();

        public SDKError CleanUp()
        {
            StopMonitor();
            return CZoomSDKeDotNetWrap.Instance.CleanUp();
        }

        public void CustomizeUI()
        {
            CMeetingConfigurationDotNetWrap.Instance.SetSharingToolbarVisibility(false);
            CMeetingUIControllerDotNetWrap.Instance.ShowSharingToolbar(false);

            CMeetingConfigurationDotNetWrap.Instance.SetBottomFloatToolbarWndVisibility(false);
            CMeetingConfigurationDotNetWrap.Instance.EnableEnterAndExitFullScreenButtonOnMeetingUI(false);
            CMeetingConfigurationDotNetWrap.Instance.EnableLButtonDBClick4SwitchFullScreenMode(false);

        }

        public SDKError EnterFullScreen()
        {
          return  CMeetingUIControllerDotNetWrap.Instance.EnterFullScreen(true,false);
        }

        public SDKError ExitFullScreen()
        {
           return CMeetingUIControllerDotNetWrap.Instance.ExitFullScreen(true, false);
        }

        public IList<DeviceModel> GetCameraList()
        {
            IList<DeviceModel> cameraList = new List<DeviceModel>();

            ICameraInfoDotNetWrap[] cameras = CVideoSettingContextDotNetWrap.Instance.GetCameraList();

            if (cameras?.Length > 0)
            {
                foreach (ICameraInfoDotNetWrap camera in cameras)
                {
                    cameraList.Add(new DeviceModel(camera.GetDeviceId(), camera.GetDeviceName(), camera.IsSelectedDevice()));
                }
            }

            return cameraList;
        }

        public IMeetingInfo GetMeetingInfo()
        {
            return CMeetingServiceDotNetWrap.Instance.GetMeetingInfo();
        }

        public SDKError GetMeetingUIWnd(ref HWNDDotNet first, ref HWNDDotNet second)
        {
            ValueType firstVT = first;
            ValueType secondVT = second;
            SDKError error = CMeetingUIControllerDotNetWrap.Instance.GetMeetingUIWnd(ref firstVT, ref secondVT);

            first = (HWNDDotNet)firstVT;
            second = (HWNDDotNet)secondVT;

            return error;
        }

        public IList<DeviceModel> GetMicList()
        {
            IList<DeviceModel> micList = new List<DeviceModel>();

            IMicInfoDotNetWrap[] mics = CAudioSettingContextDotNetWrap.Instance.GetMicList();

            if (mics?.Length > 0)
            {
                foreach (IMicInfoDotNetWrap mic in mics)
                {
                    micList.Add(new DeviceModel(mic.GetDeviceId(), mic.GetDeviceName(), mic.IsSelectedDevice()));
                }
            }

            return micList;
        }

        public uint[] GetParticipantsList()
        {
            return CMeetingParticipantsControllerDotNetWrap.Instance.GetParticipantsList();
        }

        public string GetRecordingPath()
        {
            return CRecordingSettingContextDotNetWrap.Instance.GetRecordingPath();
        }

        public IList<DeviceModel> GetSpeakerList()
        {
            IList<DeviceModel> speakerList = new List<DeviceModel>();

            ISpeakerInfoDotNetWrap[] speakers = CAudioSettingContextDotNetWrap.Instance.GetSpeakerList();

            if (speakers?.Length > 0)
            {
                foreach (ISpeakerInfoDotNetWrap speaker in speakers)
                {
                    speakerList.Add(new DeviceModel(speaker.GetDeviceId(), speaker.GetDeviceName(), speaker.IsSelectedDevice()));
                }
            }

            return speakerList;
        }

        public IUserInfoDotNetWrap GetUserByUserID(uint userId)
        {
            return CMeetingParticipantsControllerDotNetWrap.Instance.GetUserByUserID(userId);
        }

        public SDKError Initialize(InitParam initParam)
        {
            SDKError err = CZoomSDKeDotNetWrap.Instance.Initialize(initParam);
            StartMonitor();
            return err;
        }

        public SDKError Join(JoinParam joinParam)
        {
            FilterWndMessages();
            CustomizeUI();
            return CMeetingServiceDotNetWrap.Instance.Join(joinParam);
        }

        public SDKError Leave(LeaveMeetingCmd leaveMeetingCmd)
        {
            return CMeetingServiceDotNetWrap.Instance.Leave(leaveMeetingCmd);
        }

        public SDKError Login(LoginParam loginParam)
        {
            return CAuthServiceDotNetWrap.Instance.Login(loginParam);
        }

        public SDKError Logout()
        {
            return CAuthServiceDotNetWrap.Instance.LogOut();
        }

        public SDKError MonitorWnd(string name, bool add)
        {
            return CUIHookControllerDotNetWrap.Instance.MonitorWnd(name, add);
        }

        public SDKError MonitorWndMessage(uint id, bool add)
        {
            return CUIHookControllerDotNetWrap.Instance.MonitorWndMessage(id, add);
        }

        public SDKError MuteAudio(uint userId, bool allowUnmuteBySelf)
        {
            return CMeetingAudioControllerDotNetWrap.Instance.MuteAudio(userId, allowUnmuteBySelf);
        }

        public SDKError MuteVideo()
        {
            return CMeetingVideoControllerDotNetWrap.Instance.MuteVideo();
        }

        public SDKError SDKAuth(AuthParam authParam)
        {
            return CAuthServiceDotNetWrap.Instance.SDKAuth(authParam);
        }

        public SDKError SelectCamera(DeviceModel camera)
        {
            return CVideoSettingContextDotNetWrap.Instance.SelectCamera(camera.Id);
        }

        public SDKError SelectMic(DeviceModel mic)
        {
            return CAudioSettingContextDotNetWrap.Instance.SelectMic(mic.Id, mic.Name);
        }

        public SDKError SelectSpeaker(DeviceModel speaker)
        {
            return CAudioSettingContextDotNetWrap.Instance.SelectSpeaker(speaker.Id, speaker.Name);
        }

        public SDKError SetRecordingPath(string path)
        {
            return CRecordingSettingContextDotNetWrap.Instance.SetRecordingPath(path);
        }

        public SDKError Start(StartParam startParam)
        {
            FilterWndMessages();
            CustomizeUI();

            return CMeetingServiceDotNetWrap.Instance.Start(startParam);
        }

        public SDKError StartAppShare(HWNDDotNet hWNDDotNet)
        {
            return CMeetingShareControllerDotNetWrap.Instance.StartAppShare(hWNDDotNet);
        }

        public SDKError StartMonitor()
        {
            return CUIHookControllerDotNetWrap.Instance.Start();
        }

        public SDKError StartMonitorShare()
        {
            return CMeetingShareControllerDotNetWrap.Instance.StartMonitorShare();
        }

        public SDKError StartRecording(ref DateTime startTimestamp, string recPath)
        {
            ValueType valueType = startTimestamp;
            SDKError err = CMeetingRecordingControllerDotNetWrap.Instance.StartRecording(ref valueType, recPath);
            startTimestamp = (DateTime)valueType;
            return err;
        }

        public SDKError StopMonitor()
        {
            return CUIHookControllerDotNetWrap.Instance.Stop();
        }

        public SDKError StopRecording(ref DateTime stopTimestamp)
        {
            ValueType valueType = stopTimestamp;
            SDKError err = CMeetingRecordingControllerDotNetWrap.Instance.StopRecording(ref valueType);
            stopTimestamp = (DateTime)valueType;
            return err;
        }

        public SDKError StopShare()
        {
            return CMeetingShareControllerDotNetWrap.Instance.StopShare();
        }

        public SDKError UnmuteAudio(uint userId)
        {
            return CMeetingAudioControllerDotNetWrap.Instance.UnMuteAudio(userId);
        }

        public SDKError UnmuteVideo()
        {
            return CMeetingVideoControllerDotNetWrap.Instance.UnmuteVideo();
        }


        private void FilterWndMessages()
        {
            MonitorWndMessage(1, false);
            MonitorWndMessage(2, false);
            MonitorWndMessage(3, false);
            MonitorWndMessage(5, false);
            MonitorWndMessage(6, false);
            MonitorWndMessage(70, false);
            MonitorWndMessage(71, false);
        }
    }
}
