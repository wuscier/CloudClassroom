using CloudClassroom.Events;
using CloudClassroom.ViewModels;
using Prism.Events;
using System;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            SubscribeEvents();

            DataContext = new MainViewModel();
        }

        private SubscriptionToken _leaveMeetingToken;
        private SubscriptionToken _intoMeetingSuccessToken;

        private void SubscribeEvents()
        {
            _leaveMeetingToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<LeaveMeetingEvent>().Subscribe((argument) =>
            {
                Show();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.MainView; });

            _intoMeetingSuccessToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Subscribe((argument) =>
            {
                MeetingView meetingView = new MeetingView();
                meetingView.Show();

                Hide();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.MainView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<LeaveMeetingEvent>().Unsubscribe(_leaveMeetingToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Unsubscribe(_intoMeetingSuccessToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }
    }
}
