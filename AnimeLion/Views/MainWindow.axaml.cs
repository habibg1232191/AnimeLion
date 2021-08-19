using AnimeLion.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.ReactiveUI;

namespace AnimeLion.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region Controling Window
        private void WindowMoved(object? sender, PointerPressedEventArgs e)
            => BeginMoveDrag(e);
        
        private void ResizedWindow(object? sender, PointerPressedEventArgs e)
        {
            Grid? resizedGrid = (Grid?) sender;
            if(resizedGrid == null 
               & !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed
               & WindowState != WindowState.Maximized) return;
            
            switch (resizedGrid?.Name)
            {
                case "ResizeLeftWindow":
                    BeginResizeDrag(WindowEdge.West, e);
                    break;
                case "ResizeRightWindow":
                    BeginResizeDrag(WindowEdge.East, e);
                    break;
                case "ResizeTopWindow":
                    BeginResizeDrag(WindowEdge.North, e);
                    break;
                case "ResizeBottomWindow":
                    BeginResizeDrag(WindowEdge.South, e);
                    break;
                
                case "ResizeLeftTopWindow":
                    BeginResizeDrag(WindowEdge.NorthWest, e);
                    break;
                case "ResizeRightTopWindow":
                    BeginResizeDrag(WindowEdge.NorthEast, e);
                    break;
                case "ResizeRightBottomWindow":
                    BeginResizeDrag(WindowEdge.SouthEast, e);
                    break;
                case "ResizeLeftBottomWindow":
                    BeginResizeDrag(WindowEdge.SouthWest, e);
                    break;
            }
        }

        private void MaximizedWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState != WindowState.Normal ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void HideWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion
    }
}