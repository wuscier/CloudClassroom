using CloudClassroom.sdk_adapter;
using CloudClassroom.ViewModels;
using System;
using System.Windows;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.Views
{
    /// <summary>
    /// WhiteboardView.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteboardView : Window
    {
        private static readonly ISdk _sdk = ZoomSdk.Instance;

        public WhiteboardView()
        {
            InitializeComponent();
            DataContext = new WhiteboardViewModel();
        }

        protected override void OnClosed(EventArgs e)
        {
            SDKError stopShareErr = _sdk.StopShare();
        }
    }
}
