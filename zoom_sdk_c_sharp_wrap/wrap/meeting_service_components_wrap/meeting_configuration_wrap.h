#pragma once
#include "../common_include.h"
BEGIN_ZOOM_SDK_NAMESPACE
class IMeetingServiceWrap;
IMeetingConfiguration* InitIMeetingConfigurationFunc(IMeetingConfigurationEvent* pEvent, IMeetingServiceWrap* pOwner);
void UninitIMeetingConfigurationFunc(IMeetingConfiguration* obj);
BEGIN_CLASS_DEFINE_WITHCALLBACK(IMeetingConfiguration, IMeetingConfigurationEvent)
NORMAL_CLASS(IMeetingConfiguration)
INIT_UNINIT_WITHEVENT_AND_OWNSERVICE(IMeetingConfiguration, IMeetingServiceWrap)

//virtual void Reset() = 0;
DEFINE_FUNC_0(Reset, void)
//virtual void SetFloatVideoPos(WndPosition pos) = 0;
DEFINE_FUNC_1(SetFloatVideoPos, void, WndPosition, pos)
//virtual void SetSharingToolbarVisibility(bool bShow) = 0;
DEFINE_FUNC_1(SetSharingToolbarVisibility, void, bool, bShow)
//virtual void SetBottomFloatToolbarWndVisibility(bool bShow) = 0;
DEFINE_FUNC_1(SetBottomFloatToolbarWndVisibility, void, bool, bShow)
//virtual void SetDirectShareMonitorID(const wchar_t* monitorID) = 0;
DEFINE_FUNC_1(SetDirectShareMonitorID, void, const wchar_t*, monitorID)
//virtual void SetMeetingUIPos(WndPosition pos) = 0;
DEFINE_FUNC_1(SetMeetingUIPos, void, WndPosition, pos)
//virtual void DisableWaitingForHostDialog(bool bDisable) = 0;
DEFINE_FUNC_1(DisableWaitingForHostDialog, void, bool, bDisable)
//virtual void HideMeetingInfoFromMeetingUITitle(bool bHide) = 0;
DEFINE_FUNC_1(HideMeetingInfoFromMeetingUITitle, void, bool, bHide)
//virtual void SetMeetingIDForMeetingUITitle(UINT64 meetingNumber) = 0;
DEFINE_FUNC_1(SetMeetingIDForMeetingUITitle, void, UINT64, meetingNumber)
//virtual void DisablePopupMeetingWrongPSWDlg(bool bDisable) = 0;
DEFINE_FUNC_1(DisablePopupMeetingWrongPSWDlg, void, bool, bDisable)
//virtual void EnableAutoEndOtherMeetingWhenStartMeeting(bool bEnable) = 0;
DEFINE_FUNC_1(EnableAutoEndOtherMeetingWhenStartMeeting, void, bool, bEnable)
//virtual void EnableAutoAdjustSpeakerVolumeWhenJoinAudio(bool bEnable) = 0;
DEFINE_FUNC_1(EnableAutoAdjustSpeakerVolumeWhenJoinAudio, void, bool, bEnable)
//virtual void EnableAutoAdjustMicVolumeWhenJoinAudio(bool bEnable) = 0;
DEFINE_FUNC_1(EnableAutoAdjustMicVolumeWhenJoinAudio, void, bool, bEnable)
//virtual void EnableApproveRemoteControlDlg(bool bEnable) = 0;
DEFINE_FUNC_1(EnableApproveRemoteControlDlg, void, bool, bEnable)
//virtual void EnableDeclineRemoteControlResponseDlg(bool bEnable) = 0;
DEFINE_FUNC_1(EnableDeclineRemoteControlResponseDlg, void, bool, bEnable)
//virtual void EnableLeaveMeetingOptionForHost(bool bEnable) = 0;
DEFINE_FUNC_1(EnableLeaveMeetingOptionForHost, void, bool, bEnable)
//virtual void EnableInviteButtonOnMeetingUI(bool bEnable) = 0;
DEFINE_FUNC_1(EnableInviteButtonOnMeetingUI, void, bool, bEnable)
//virtual void EnableInputMeetingPasswordDlg(bool bEnable) = 0;
DEFINE_FUNC_1(EnableInputMeetingPasswordDlg, void, bool, bEnable)
//virtual void EnableEnterAndExitFullScreenButtonOnMeetingUI(bool bEnable) = 0;
DEFINE_FUNC_1(EnableEnterAndExitFullScreenButtonOnMeetingUI, void, bool, bEnable)
//virtual void EnableLButtonDBClick4SwitchFullScreenMode(bool bEnable) = 0;
DEFINE_FUNC_1(EnableLButtonDBClick4SwitchFullScreenMode, void, bool, bEnable)
//virtual void SetFloatVideoWndVisibility(bool bShow) = 0;
DEFINE_FUNC_1(SetFloatVideoWndVisibility, void, bool, bShow)
//virtual void PrePopulateWebinarRegistrationInfo(const wchar_t* email, const wchar_t* username) = 0;
DEFINE_FUNC_2(PrePopulateWebinarRegistrationInfo, void, const wchar_t*, email, const wchar_t*, username)
//virtual void RedirectClickShareBTNEvent(bool bRedirect) = 0;
DEFINE_FUNC_1(RedirectClickShareBTNEvent, void, bool, bRedirect)
//virtual void RedirectClickEndMeetingBTNEvent(bool bRedirect) = 0;
DEFINE_FUNC_1(RedirectClickEndMeetingBTNEvent, void, bool, bRedirect)
//virtual void EnableToolTipsShow(bool bEnable) = 0;
DEFINE_FUNC_1(EnableToolTipsShow, void, bool, bEnable)
//virtual void EnableAirplayInstructionWindow(bool bEnable) = 0;
DEFINE_FUNC_1(EnableAirplayInstructionWindow, void, bool, bEnable)
//virtual void EnableClaimHostFeature(bool bEnable) = 0;
DEFINE_FUNC_1(EnableClaimHostFeature, void, bool, bEnable)

//virtual void onInputMeetingPasswordAndScreenNameNotification(IMeetingPasswordAndScreenNameHandler* pHandler) = 0;
CallBack_FUNC_1(onInputMeetingPasswordAndScreenNameNotification, IMeetingPasswordAndScreenNameHandler*, pHandler)
//virtual void onAirPlayInstructionWndNotification(bool bShow, const wchar_t* airhostName) = 0;
CallBack_FUNC_2(onAirPlayInstructionWndNotification, bool, bShow, const wchar_t*, airhostName)
END_CLASS_DEFINE(IMeetingConfiguration)

END_ZOOM_SDK_NAMESPACE