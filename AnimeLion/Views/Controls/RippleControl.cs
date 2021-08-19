using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;


public delegate void MPointerReleasedEvent(object? o, PointerReleasedEventArgs args);
public delegate void MPointerEvent(object? o, PointerEventArgs args);

namespace AnimeLion.Views.Controls
{
    public static class RippleControl
    {
        public static void RippleStart(object? sender, PointerPressedEventArgs e, Canvas mainGrid, Size size,
            SolidColorBrush rippleFill, double rippleOpacity)
        {
            Control btn = (Control) sender!;
            Ellipse circle = new Ellipse()
            {
                Fill = rippleFill, Opacity = rippleOpacity
            };
            Grid spanCircle = new Grid();
            Point pos = e.GetPosition(mainGrid);
            double radius = Math.Abs(size.Height/2 - pos.Y) + pos.X * 2;

            // Left Top
            if (pos.X <= size.Width/2 && pos.Y <= size.Height/2)
                radius = Math.Sqrt(Math.Pow(size.Width - pos.X, 2) + Math.Pow(size.Height - pos.Y, 2));
            // Right Top
            else if (pos.X >= size.Width / 2 && pos.Y <= size.Height / 2)
                radius = Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(Math.Abs(pos.Y - size.Height), 2));
            // Left Bottom
            else if (pos.X <= size.Width / 2 && pos.Y >= size.Height / 2)
                radius = Math.Sqrt(Math.Pow(pos.X - size.Width, 2) + Math.Pow(Math.Abs(pos.Y), 2));
            // Right Bottom
            else if (pos.X >= size.Width / 2 && pos.Y >= size.Height / 2)
                radius = Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(Math.Abs(pos.Y), 2));

            radius *= 2;
            
            circle.Classes.Add("Ripple");
            
            circle.VerticalAlignment = VerticalAlignment.Top;
            circle.HorizontalAlignment = HorizontalAlignment.Left;
            circle.RenderTransform = new TranslateTransform(pos.X, pos.Y);
            circle.Height = circle.Width = 0;
            
            spanCircle.Children.Add(circle);
            mainGrid.Children.Add(spanCircle);
            circle.Height = circle.Width = radius;
            circle.Margin = new Thickness(-radius / 2, -radius / 2, 0, 0);
            
            MPointerReleasedEvent removeRipple = null!;
            removeRipple = (sender, e) =>
            {
                circle.Opacity = 0;
                btn.RemoveHandler(InputElement.PointerReleasedEvent, removeRipple);
                DispatcherTimer.Run(() =>
                {
                    spanCircle.Children.Clear();
                    mainGrid.Children.Remove(spanCircle);
                    return false;
                }, TimeSpan.FromMilliseconds(400));
            };
            btn.AddHandler(InputElement.PointerReleasedEvent, removeRipple, RoutingStrategies.Tunnel);
        }
    }
}