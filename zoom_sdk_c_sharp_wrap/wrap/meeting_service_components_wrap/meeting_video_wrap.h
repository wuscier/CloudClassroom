#pragma once
#include "../common_include.h"
BEGIN_ZOOM_SDK_NAMESPACE
class IMeetingServiceWrap;
IMeetingVideoController* InitIMeetingVideoControllerFunc(IMeetingVideoCtrlEvent* pEvent, IMeetingServiceWrap* pOwner);
void UninitIMeetingVideoControllerFunc(IMeetingVideoController* obj);
BEGIN_CLASS_DEFINE_WITHCALLBACK(IMeetingVideoController, IMeetingVideoCtrlEvent)
NORMAL_CLASS(IMeetingVideoController)
INIT_UNINIT_WITHEVENT_AND_OWNSERVICE(IMeetingVideoController, IMeetingServiceWrap)

//virtual SDKError MuteVideo() = 0;
DEFINE_FUNC_0(MuteVideo, SDKError)
//virtual SDKError UnmuteVideo() = 0;
DEFINE_FUNC_0(UnmuteVideo, SDKError)
//virtual SDKError PinVideo(bool bPin, bool bFirstView, unsigned int userid) = 0;
DEFINE_FUNC_3(PinVideo, SDKError, bool, bPin, bool, bFirstView, unsigned int, userid)
//virtual SDKError SpotlightVideo(bool bSpotlight, unsigned int userid) = 0;
DEFINE_FUNC_2(SpotlightVideo, SDKError, bool, bSpotlight, unsigned int, userid)
//virtual SDKError HideOrShowNoVideoUserOnVideoWall(bool bHide) = 0;
DEFINE_FUNC_1(HideOrShowNoVideoUserOnVideoWall, SDKError, bool, bHide)

//virtual void onUserVideoStatusChange(unsigned int userId, VideoStatus status) = 0;
CallBack_FUNC_2(onUserVideoStatusChange, unsigned int, userId, VideoStatus, status)
//virtual void onSpotlightVideoChangeNotification(bool bSpotlight, unsigned int userid) = 0;
CallBack_FUNC_2(onSpotlightVideoChangeNotification, bool, bSpotlight, unsigned int, userid)
END_CLASS_DEFINE(IMeetingVideoController)
END_ZOOM_SDK_NAMESPACE