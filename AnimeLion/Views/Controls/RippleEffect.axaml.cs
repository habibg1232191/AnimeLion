using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AnimeLion.Views.Controls
{
    public class RippleEffect : ContentControl
    {
        public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
            AvaloniaProperty.Register<RippleEffect, CornerRadius>(nameof(CornerRadius));
        
        public static readonly StyledProperty<SolidColorBrush> RippleFillProperty =
            AvaloniaProperty.Register<RippleEffect, SolidColorBrush>(nameof(RippleFill));
        
        public static readonly StyledProperty<double> RippleOpacityProperty =
            AvaloniaProperty.Register<RippleEffect, double>(nameof(RippleOpacity));

        public CornerRadius CornerRadius
        {
            get => GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public SolidColorBrush RippleFill
        {
            get => GetValue(RippleFillProperty);
            set => SetValue(RippleFillProperty, value);
        }
        
        public double RippleOpacity
        {
            get => GetValue(RippleOpacityProperty);
            set => SetValue(RippleOpacityProperty, value);
        }
        
        private Canvas _rippleCanvas;

        public RippleEffect()
        {
            _rippleCanvas = new Canvas();
            AddHandler(PointerPressedEvent, PointerPress, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        }
        
        private void PointerPress(object? sender, PointerPressedEventArgs e)
        {
            if(e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed) return;
            
            RippleControl.RippleStart(sender, e, _rippleCanvas, _rippleCanvas.DesiredSize,
                RippleFill, RippleOpacity);
        }
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _rippleCanvas = e.NameScope.Find<Canvas>("RippleCanvas");
        }
    }
}