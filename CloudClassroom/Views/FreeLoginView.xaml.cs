using CloudClassroom.ViewModels;
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
            DataContext = new FreeLoginViewModel();
        }
    }
}
