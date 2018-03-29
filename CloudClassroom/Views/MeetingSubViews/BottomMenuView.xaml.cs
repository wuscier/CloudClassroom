using CloudClassroom.ViewModels;
using System.Windows.Controls;

namespace CloudClassroom.Views
{
    /// <summary>
    /// BottomMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class BottomMenuView : UserControl
    {
        public BottomMenuView()
        {
            InitializeComponent();
            DataContext = new BottomMenuViewModel();
        }
    }
}
