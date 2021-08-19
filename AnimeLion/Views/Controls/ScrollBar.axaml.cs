using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AnimeLion.Views.Controls
{
    public class ScrollBar : ContentControl
    {
        private Border? _partBorder;

        public static readonly StyledProperty<double> ItemWidthProperty =
            AvaloniaProperty.Register<RippleEffect, double>(nameof(ItemWidth));

        public double ItemWidth
        {
            get => GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }
        
        public ScrollBar()
        {
            AddHandler(PointerWheelChangedEvent, OnPointerWheelChanged,
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        }
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partBorder = e.NameScope.Find<Border>("PART_Border");
        }

        private double scrollBarOffsetX = 0;

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            if (_partBorder == null) return;
            var childBorder = (Control) _partBorder.Child;
            
            Console.WriteLine(scrollBarOffsetX);
            
            if (e.Delta.Y > 0)
            {
                if (-e.Delta.Y * (ItemWidth / 2) - scrollBarOffsetX >= Bounds.Width)
                {
                    childBorder.Margin =
                        new Thickness(Bounds.Width, _partBorder.Margin.Top,
                            _partBorder.Margin.Right, _partBorder.Margin.Bottom);
                    scrollBarOffsetX = 0;
                    return;
                }
            }
            else if (e.Delta.Y < 0)
            {
                if (-e.Delta.Y * (ItemWidth / 2) - scrollBarOffsetX <= ItemWidth / 2)
                {
                    childBorder.Margin =
                        new Thickness(0, _partBorder.Margin.Top,
                            _partBorder.Margin.Right, _partBorder.Margin.Bottom);
                    scrollBarOffsetX = 0;
                    return;
                }
            }
            
            scrollBarOffsetX += -e.Delta.Y * (ItemWidth / 2);
            

            childBorder.Margin =
                new Thickness(scrollBarOffsetX, _partBorder.Margin.Top,
                    _partBorder.Margin.Right, _partBorder.Margin.Bottom);
        }
    }
}