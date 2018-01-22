#include "meeting_video_wrap.h"
#include "../meeting_service_wrap.h"
BEGIN_ZOOM_SDK_NAMESPACE
IMeetingVideoController* InitIMeetingVideoControllerFunc(IMeetingVideoCtrlEvent* pEvent, IMeetingServiceWrap* pOwner)
{
	if (pOwner && pOwner->GetSDKObj())
	{
		ZOOM_SDK_NAMESPACE::IMeetingVideoController* pObj = pOwner->GetSDKObj()->GetMeetingVideoController();
		if (pObj)
		{
			pObj->SetEvent(pEvent);
		}
		return pObj;
	}

	return NULL;
}

void UninitIMeetingVideoControllerFunc(IMeetingVideoController* obj)
{
	if (obj)
	{
		obj->SetEvent(NULL);
	}
}

//virtual SDKError MuteVideo() = 0;
IMPL_FUNC_0(IMeetingVideoController, MuteVideo, SDKError, SDKERR_UNINITIALIZE)
//virtual SDKError UnmuteVideo() = 0;
IMPL_FUNC_0(IMeetingVideoController, UnmuteVideo, SDKError, SDKERR_UNINITIALIZE)
//virtual SDKError PinVideo(bool bPin, bool bFirstView, unsigned int userid) = 0;
IMPL_FUNC_3(IMeetingVideoController, PinVideo, SDKError, bool, bPin, bool, bFirstView, unsigned int, userid, SDKERR_UNINITIALIZE)
//virtual SDKError SpotlightVideo(bool bSpotlight, unsigned int userid) = 0;
IMPL_FUNC_2(IMeetingVideoController, SpotlightVideo, SDKError, bool, bSpotlight, unsigned int, userid, SDKERR_UNINITIALIZE)
//virtual SDKError HideOrShowNoVideoUserOnVideoWall(bool bHide) = 0;
IMPL_FUNC_1(IMeetingVideoController, HideOrShowNoVideoUserOnVideoWall, SDKError, bool, bHide, SDKERR_UNINITIALIZE)
END_ZOOM_SDK_NAMESPACE