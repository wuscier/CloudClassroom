#include "sdk_wrap.h"
#include "sdk_loader.h"
BEGIN_ZOOM_SDK_NAMESPACE
CSDKWrap& CSDKWrap::GetInst()
{
	static CSDKWrap wrap;
	return wrap;
}

SDKError CSDKWrap::InitSDK(const wchar_t* sdkpath, InitParam& initParam)
{
	SDKError sdkerr(SDKERR_INVALID_PARAMETER);
	do 
	{
		std::wstring path = sdkpath ? sdkpath : L"";
		if (!CSDKImpl::GetInst().ConfigSDKModule(path))
		{
			break;
		}

		sdkerr = CSDKImpl::GetInst().InitSDK(initParam);
	} while (false);

	return sdkerr;
}

SDKError CSDKWrap::CleanUPSDK()
{
	return CSDKImpl::GetInst().CleanUPSDK();
}

const wchar_t* CSDKWrap::GetVersion()
{
	return CSDKImpl::GetInst().GetVersion();
}

IAuthServiceWrap& CSDKWrap::GetAuthServiceWrap()
{
	return IAuthServiceWrap::GetInst();
}

IMeetingServiceWrap& CSDKWrap::GetMeetingServiceWrap()
{
	return IMeetingServiceWrap::GetInst();
}

ICalenderServiceWrap& CSDKWrap::GetCalenderServiceWrap()
{
	return ICalenderServiceWrap::GetInst();
}

IPreMeetingServiceWrap& CSDKWrap::GetPreMeetingServiceWrap()
{
	return IPreMeetingServiceWrap::GetInst();
}

ISettingServiceWrap& CSDKWrap::GetSettingServiceWrap()
{
	return ISettingServiceWrap::GetInst();
}

INetworkConnectionHelperWrap& CSDKWrap::GetNetworkConnectionHelperWrap()
{
	return INetworkConnectionHelperWrap::GetInst();
}

CSDKWrap::CSDKWrap()
{
}
/////////////////////////////////////////////////////////////////////
CSDKExtWrap::CSDKExtWrap()
{

}

CSDKExtWrap& CSDKExtWrap::GetInst()
{
	static CSDKExtWrap inst;
	return inst;
}

IUIHookerWrap& CSDKExtWrap::GetUIHookerWrap()
{
	return IUIHookerWrap::GetInst();
}

IEmbeddedBrowserWrap* CSDKExtWrap::CreateEmbeddedBrowserWrap(HWND hwnd)
{
	IEmbeddedBrowserWrap* pObj(NULL);
	if (hwnd)
	{
		pObj = new IEmbeddedBrowserWrap;
		if (pObj)
		{
			pObj->Init(hwnd);
		}
	}
	
	return pObj;
}

void CSDKExtWrap::DestroyEmbeddedBrowserWrap(IEmbeddedBrowserWrap* pObj)
{
	if (pObj)
	{
		pObj->Uninit();
		delete pObj;
	}
}
END_ZOOM_SDK_NAMESPACE