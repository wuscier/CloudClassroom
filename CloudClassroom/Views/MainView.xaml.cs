using CloudClassroom.Events;
using CloudClassroom.Models;
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

        private SubscriptionToken _intoMeetingSuccessToken;
        private SubscriptionToken _showLessonDetailToken;

        private void SubscribeEvents()
        {
            _intoMeetingSuccessToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Subscribe((argument) =>
            {
                Hide();

                MeetingView meetingView = new MeetingView();
                meetingView.Show();

            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.MainView; });

            _showLessonDetailToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowLessonDetailEvent>().Subscribe((argument) =>
            {
                LessonModel lessonModel = argument.Argument.Value as LessonModel;
                LessonDetailView lessonDetailView = new LessonDetailView(lessonModel);
                lessonDetailView.Owner = this;
                lessonDetailView.ShowDialog();

            }, ThreadOption.PublisherThread, true, filter => { return filter.Target == Target.MainView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<StartOrJoinSuccessEvent>().Unsubscribe(_intoMeetingSuccessToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<ShowLessonDetailEvent>().Unsubscribe(_showLessonDetailToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }
    }
}
