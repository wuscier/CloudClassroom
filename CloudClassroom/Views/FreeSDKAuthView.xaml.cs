using CloudClassroom.Events;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// FreeSDKAuthView.xaml 的交互逻辑
    /// </summary>
    public partial class FreeSDKAuthView : Window
    {

        private SubscriptionToken _sdkAuthToken;

        public FreeSDKAuthView()
        {
            InitializeComponent();

            SubscribeEvents();

            DataContext = new FreeSDKAuthViewModel();
        }

        private void SubscribeEvents()
        {
            _sdkAuthToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<SDKAuthSuccessEvent>().Subscribe((argument) =>
            {
                FreeLoginView freeLoginView = new FreeLoginView();
                freeLoginView.Show();

                Close();
            }, ThreadOption.UIThread,true,filter=> { return filter.Target == Target.FreeSDKAuthView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Unsubscribe(_sdkAuthToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }

    }
}
