using CloudClassroom.ViewModels;
using System.Windows;

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
