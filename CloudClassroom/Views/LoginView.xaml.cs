using CloudClassroom.Events;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        private SubscriptionToken _windowCloseToken;

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

            _windowCloseToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowCloseEvent>().Subscribe((argument) =>
            {
                if (App.MainView == null)
                {
                    App.MainView = new MainView();
                }

                App.MainView.Show();

                Close();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.LoginView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<UIGotFocusEvent>().Unsubscribe(_uiGotFocusToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowCloseEvent>().Unsubscribe(_windowCloseToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }

    }
}
