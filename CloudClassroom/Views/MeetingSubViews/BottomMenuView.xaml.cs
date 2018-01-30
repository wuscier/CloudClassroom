using System;
using System.ComponentModel;
using System.Windows;

namespace CloudClassroom.Views
{
    /// <summary>
    /// BottomMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class BottomMenuView : Window
    {
        public bool IsClosed { get; private set; }

        public BottomMenuView()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            IsClosed = true;
        }
    }
}
