using CloudClassroom.Events;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            SubscribeEvents();
            DataContext = new LoginViewModel();
        }

        private SubscriptionToken _uiGotFocusToken;
        private SubscriptionToken _loginSuccessToken;

        private void SubscribeEvents()
        {
            _uiGotFocusToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<UIGotFocusEvent>().Subscribe((argument) =>
            {
                switch (argument.Argument.Category)
                {
                    case Category.UserName:
                        username.Focus();
                        break;
                    case Category.Password:
                        password.Focus();
                        break;
                }
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.LoginView; });

            _loginSuccessToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Subscribe((argument) =>
            {
                App.MainView = new MainView();
                App.MainView.Show();

                App.BottomMenuViewModel = new BottomMenuViewModel();

                Close();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.LoginView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<UIGotFocusEvent>().Unsubscribe(_uiGotFocusToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<LoginSuccessEvent>().Unsubscribe(_loginSuccessToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }

    }
}
