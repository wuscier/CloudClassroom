#include "Stdafx.h"
#include "ui_hook_dotnet_wrap.h"
#include "zoom_sdk_dotnet_wrap_util.h"
#include "wrap\sdk_wrap.h"

namespace ZOOM_SDK_DOTNET_WRAP {

	class UIHookControllerEventHanlder {
	public:
		static UIHookControllerEventHanlder& GetInst() {
			static UIHookControllerEventHanlder inst;
			return inst;
		}

		virtual void onUIActionNotify(ZOOM_SDK_NAMESPACE::UIHOOKHWNDTYPE type, MSG msg) {
			if (CUIHookControllerDotNetWrap::Instance)
			{
				CUIHookControllerDotNetWrap::Instance->procUIActionNotify((UIHOOKHWNDTYPE)type, msg.message);
			}
		}

	private:
		UIHookControllerEventHanlder() {}
	};

	SDKError CUIHookControllerDotNetWrap::MonitorWndMessage(unsigned int wndMsgId, bool bAdd)
	{
		SDKError err = (SDKError)ZOOM_SDK_NAMESPACE::CSDKExtWrap::GetInst().GetUIHookerWrap().MonitorWndMessage(wndMsgId, bAdd);
		return err;
	}

	SDKError CUIHookControllerDotNetWrap::MonitorWnd(String ^ className, bool bAdd)
	{
		SDKError err = (SDKError)ZOOM_SDK_NAMESPACE::CSDKExtWrap::GetInst().GetUIHookerWrap().MonitorWnd(const_cast<wchar_t*>(PlatformString2WChar(className)), bAdd);
		return err;
	}

	SDKError CUIHookControllerDotNetWrap::Start()
	{
		SDKError err = (SDKError)ZOOM_SDK_NAMESPACE::CSDKExtWrap::GetInst().GetUIHookerWrap().Start();
		return err;
	}

	SDKError CUIHookControllerDotNetWrap::Stop()
	{
		SDKError err = (SDKError)ZOOM_SDK_NAMESPACE::CSDKExtWrap::GetInst().GetUIHookerWrap().Stop();
		return err;
	}

	void CUIHookControllerDotNetWrap::BindEvent()
	{
		ZOOM_SDK_NAMESPACE::CSDKExtWrap::GetInst().GetUIHookerWrap().m_cbonUIActionNotify = std::bind(&UIHookControllerEventHanlder::onUIActionNotify,
			&UIHookControllerEventHanlder::GetInst(), std::placeholders::_1, std::placeholders::_2);
	}

	void CUIHookControllerDotNetWrap::procUIActionNotify(UIHOOKHWNDTYPE type, unsigned int msg)
	{
		event_onUIActionNotify(type, msg);
	}
}
