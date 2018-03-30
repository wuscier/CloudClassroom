using CloudClassroom.Events;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// FreeLoginView.xaml 的交互逻辑
    /// </summary>
    public partial class FreeLoginView : Window
    {
        public FreeLoginView()
        {
            InitializeComponent();
            SubscribeEvents();

            DataContext = new FreeLoginViewModel();
        }


        private SubscriptionToken _loginSuccessToken;


        private void SubscribeEvents()
        {
            _loginSuccessToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Subscribe((argument) =>
            {
                App.MainView = new MainView();
                App.MainView.Show();

                //App.BottomMenuViewModel = new BottomMenuViewModel();

                Close();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.FreeLoginView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Unsubscribe(_loginSuccessToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }

    }
}
