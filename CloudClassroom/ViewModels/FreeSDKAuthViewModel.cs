using CloudClassroom.Events;
using CloudClassroom.sdk_adapter;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.ViewModels
{
    public class FreeSDKAuthViewModel:BindableBase
    {
        private readonly ISdk _sdk;


        public FreeSDKAuthViewModel()
        {
            RegisterCallbacks();

            SdkKey = "LgNeAkuV4Ns4MR9xeVtLLZMFwC7dffsdGrKI";
            SdkSecret = "eYjCcfZrgbULGGHLiRRsHTQSx84q8cQZlybj";

            _sdk = ZoomSdk.Instance;

            SDKAuthCommand = new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(SdkKey))
                {
                    SdkAuthStatus = "请填写SDK key！";
                    return;
                };
                if (string.IsNullOrEmpty(SdkSecret))
                {
                    SdkAuthStatus = "请填写SDK secret！";
                    return;
                };

                _sdk.SDKAuth(new AuthParam()
                {
                    appKey = SdkKey,
                    appSecret = SdkSecret,
                });

            });

        }


        private string _sdkKey;

        public string SdkKey
        {
            get { return _sdkKey; }
            set { SetProperty(ref _sdkKey, value); }
        }
        private string _sdkSecret;

        public string SdkSecret
        {
            get { return _sdkSecret; }
            set { SetProperty(ref _sdkSecret, value); }
        }
        private string _sdkAuthStatus;

        public string SdkAuthStatus
        {
            get { return _sdkAuthStatus; }
            set { SetProperty(ref _sdkAuthStatus, value); }
        }

        public ICommand SDKAuthCommand { get; set; }


        private void RegisterCallbacks()
        {
            CZoomSDKeDotNetWrap.Instance.GetAuthServiceWrap().Add_CB_onAuthenticationReturn((authResult) =>
            {
                SdkAuthStatus = authResult.ToString();

                if (authResult == AuthResult.AUTHRET_SUCCESS)
                {
                    EventAggregatorManager.Instance.EventAggregator.GetEvent<SDKAuthSuccessEvent>().Publish(new EventArgument()
                    {
                        Target = Target.FreeSDKAuthView,
                    });
                }
            });

        }

    }
}
