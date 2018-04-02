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

        private SubscriptionToken _intoMeetingSuccessToken;


        private void SubscribeEvents()
        {
            _intoMeetingSuccessToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Subscribe((argument) =>
            {
                Hide();

                MeetingView meetingView = new MeetingView();
                meetingView.Show();

            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.FreeLoginView; });

        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Unsubscribe(_intoMeetingSuccessToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }

    }
}
