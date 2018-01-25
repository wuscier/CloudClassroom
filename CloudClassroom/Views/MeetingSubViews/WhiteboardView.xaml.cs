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

            ink_canvas.DefaultDrawingAttributes.Color = Colors.Red;
            ink_canvas.DefaultDrawingAttributes.Width = 3;
            ink_canvas.DefaultDrawingAttributes.Height = 3;
        }

        private void note_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (note_detail.Visibility == Visibility.Visible)
            {
                note_detail.Visibility = Visibility.Collapsed;
            }
            else
            {
                note_detail.Visibility = Visibility.Visible;
            }
        }

        private void thumbnail_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (thumbnail_detail.Visibility == Visibility.Visible)
            {
                thumbnail_detail.Visibility = Visibility.Collapsed;
            }
            else
            {
                thumbnail_detail.Visibility = Visibility.Visible;
            }
        }

        private void next_page_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<NextPageEvent>().Publish(new EventArgument()
            {
                Target = Target.WhiteboardViewModel,
            });
        }

        private void previous_page_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<PreviousPageEvent>().Publish(new EventArgument()
            {
                Target = Target.WhiteboardViewModel,
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            WhiteboardViewModel whiteboardViewModel = DataContext as WhiteboardViewModel;

            whiteboardViewModel.UnsubscribeEvents();
        }

        private void pen_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ink_canvas.EditingMode = InkCanvasEditingMode.Ink;
            EventAggregatorManager.Instance.EventAggregator.GetEvent<PenSelectedEvent>().Publish(new EventArgument()
            {
                Target = Target.WhiteboardViewModel,
            });

        }

        private void eraser_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ink_canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            EventAggregatorManager.Instance.EventAggregator.GetEvent<EraserSelectedEvent>().Publish(new EventArgument()
            {
                Target = Target.WhiteboardViewModel,
            });

        }

        private void color_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            color_selected.Background = border.Background;

            SolidColorBrush solidColorBrush = color_selected.Background as SolidColorBrush;
            ink_canvas.DefaultDrawingAttributes.Color = solidColorBrush.Color;
        }

        private void thickness_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            TextBlock textBlock = stackPanel.Children[1] as TextBlock;
            thickness_number.Text = textBlock.Text;

            int thickness = int.Parse(thickness_number.Text);
            ink_canvas.DefaultDrawingAttributes.Height = thickness + 2;
            ink_canvas.DefaultDrawingAttributes.Width = thickness + 2;
        }

        private void clear_card_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EventAggregatorManager.Instance.EventAggregator.GetEvent<StrokesClearedEvent>().Publish(new EventArgument()
            {
                Target = Target.WhiteboardViewModel,
            });
        }
    }
}
