﻿using CloudClassroom.Events;
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

        private SubscriptionToken _windowShowToken;
        private SubscriptionToken _windowHideToken;

        private void SubscribeEvents()
        {
            _windowShowToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowShowEvent>().Subscribe((argument) =>
            {
                Show();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.MainView; });

            _windowHideToken = EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowHideEvent>().Subscribe((argument) =>
            {
                Hide();
            }, ThreadOption.UIThread, true, filter => { return filter.Target == Target.MainView; });
        }

        private void UnsubscribeEvents()
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowShowEvent>().Unsubscribe(_windowShowToken);
            EventAggregatorManager.Instance.EventAggregator.GetEvent<WindowHideEvent>().Unsubscribe(_windowHideToken);
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
        }
    }
}
