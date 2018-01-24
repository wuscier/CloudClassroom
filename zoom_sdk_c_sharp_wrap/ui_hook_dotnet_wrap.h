#pragma once
using namespace System;
#include "zoom_sdk_dotnet_wrap_def.h"

namespace ZOOM_SDK_DOTNET_WRAP {
	public enum class UIHOOKHWNDTYPE {
		UIHOOKWNDTYPE_USERDEFIEND,
		UIHOOKWNDTYPE_MAINWND,
		UIHOOKWNDTYPE_BOTTOMTOOLBAR,
	};

	public delegate void onUIActionNotify(UIHOOKHWNDTYPE type, unsigned int msg);

	public interface class IUIHookControllerDotNetWrap {
		SDKError MonitorWndMessage(unsigned int wndMsgId, bool bAdd);
		SDKError MonitorWnd(String^ className, bool bAdd);
		SDKError Start();
		SDKError Stop();

		void Add_CB_onUIActionNotify(onUIActionNotify^ cb);
	};

	public ref class CUIHookControllerDotNetWrap sealed:public IUIHookControllerDotNetWrap {
	public:
		static property CUIHookControllerDotNetWrap^ Instance {
			CUIHookControllerDotNetWrap^ get() { return m_Instance; }
		}

		virtual SDKError MonitorWndMessage(unsigned int wndMsgId, bool bAdd);
		virtual SDKError MonitorWnd(String^ className, bool bAdd);
		virtual SDKError Start();
		virtual SDKError Stop();

		virtual void Add_CB_onUIActionNotify(onUIActionNotify^ cb) {
			event_onUIActionNotify += cb;
		}

		void BindEvent();
		void procUIActionNotify(UIHOOKHWNDTYPE type, unsigned int msg);

	private:
		event onUIActionNotify^ event_onUIActionNotify;
		static CUIHookControllerDotNetWrap^ m_Instance = gcnew CUIHookControllerDotNetWrap;
	};

}
