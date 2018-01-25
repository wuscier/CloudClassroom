using Classroom.ViewModels;
using CloudClassroom.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CloudClassroom.Views
{
    /// <summary>
    /// WhiteboardView.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteboardView : Window
    {
        public WhiteboardView()
        {
            InitializeComponent();
            DataContext = new WhiteboardViewModel();
        }
    }
}
