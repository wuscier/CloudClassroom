#pragma once
#include "../common_include.h"
BEGIN_ZOOM_SDK_NAMESPACE
class IMeetingServiceWrap;
IMeetingH323Helper* InitIMeetingH323HelperFunc(IMeetingH323HelperEvent* pEvent, IMeetingServiceWrap* pOwner);
void UninitIMeetingH323HelperFunc(IMeetingH323Helper* obj);
BEGIN_CLASS_DEFINE_WITHCALLBACK(IMeetingH323Helper, IMeetingH323HelperEvent)
NORMAL_CLASS(IMeetingH323Helper)
INIT_UNINIT_WITHEVENT_AND_OWNSERVICE(IMeetingH323Helper, IMeetingServiceWrap)

//virtual IList<const wchar_t* >* GetH323Address() = 0;
DEFINE_FUNC_0(GetH323Address, IList<const wchar_t* >*)
//virtual const wchar_t* GetH323Password() = 0;
DEFINE_FUNC_0(GetH323Password, const wchar_t*)
//virtual SDKError SendMeetingParingCode(UINT64 meetingNum, wchar_t* paringCode) = 0;
DEFINE_FUNC_2(SendMeetingParingCode, SDKError, UINT64, meetingNum, wchar_t*, paringCode)
//virtual SDKError CallOutH323(H323Device& deviceInfo) = 0;
DEFINE_FUNC_1(CallOutH323, SDKError, H323Device&, deviceInfo)
//virtual SDKError CancelCallOutH323() = 0;
DEFINE_FUNC_0(CancelCallOutH323, SDKError)

//virtual void onCalloutStatusNotify(H323CalloutStatus status) = 0;
CallBack_FUNC_1(onCalloutStatusNotify, H323CalloutStatus, status)
//virtual void onParingH323Result(H323ParingResult result) = 0;
CallBack_FUNC_1(onParingH323Result, H323ParingResult, result)
END_CLASS_DEFINE(IMeetingH323Helper)
END_ZOOM_SDK_NAMESPACE