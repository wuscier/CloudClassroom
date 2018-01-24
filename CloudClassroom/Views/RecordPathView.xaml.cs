using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.Views
{
    /// <summary>
    /// RecordPathView.xaml 的交互逻辑
    /// </summary>
    public partial class RecordPathView : Window
    {
        public RecordPathView()
        {
            InitializeComponent();
        }

        private void save_record_path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SDKError error = CRecordingSettingContextDotNetWrap.Instance.SetRecordingPath(record_path.Text);
            Close();
        }

        private void record_path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            record_path.Text = folderBrowserDialog.SelectedPath.Trim();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            record_path.Text = CRecordingSettingContextDotNetWrap.Instance.GetRecordingPath();
        }
    }
}
