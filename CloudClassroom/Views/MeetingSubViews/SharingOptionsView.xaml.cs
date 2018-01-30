using CloudClassroom.Helpers;
using CloudClassroom.sdk_adapter;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.Views
{
    /// <summary>
    /// SharingOptionsView.xaml 的交互逻辑
    /// </summary>
    public partial class SharingOptionsView : Window
    {
        public SharingOptionsView()
        {
            InitializeComponent();
        }

        private void whiteboard_card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WhiteboardView whiteboardView = new WhiteboardView();
            whiteboardView.Show();

            int handle = new WindowInteropHelper(whiteboardView).Handle.ToInt32();
            HWNDDotNet whiteboardHwnd = new HWNDDotNet() { value = (uint)handle };
            SDKError shareBoardErr = ZoomSdk.Instance.StartAppShare(whiteboardHwnd);

            if (shareBoardErr != SDKError.SDKERR_SUCCESS)
            {
                whiteboardView.Close();
            }
            else
            {
                Close();
            }
        }

        private void document_card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void desktop_card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SDKError shareMonitorErr = ZoomSdk.Instance.StartMonitorShare();

            if (shareMonitorErr != SDKError.SDKERR_SUCCESS)
            {
                MessageBox.Show(Translator.TranslateSDKError(shareMonitorErr));
            }
            else
            {

                Close();
            }
        }
    }
}
