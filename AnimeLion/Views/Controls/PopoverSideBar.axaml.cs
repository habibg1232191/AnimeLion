using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace AnimeLion.Views.Controls
{
    public class PopoverSideBar : ContentControl
    {
        public static readonly StyledProperty<string> TextTooltipProperty = 
            AvaloniaProperty.Register<PopoverSideBar, string>(nameof(TextTooltip));
        
        public string TextTooltip
        {
            get => GetValue(TextTooltipProperty);
            set => SetValue(TextTooltipProperty, value);
        }

        private Popup? _tooltipPopover;
        private ContentPresenter? contentPresenter;
        private bool isOpen = false;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _tooltipPopover = e.NameScope.Find<Popup>("TooltipPopup");
            contentPresenter = e.NameScope.Find<ContentPresenter>("PART_ContentPresenter");
        }

        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            if(_tooltipPopover == null || contentPresenter == null || _tooltipPopover.Child == null) return;
            
            _tooltipPopover.HorizontalOffset = contentPresenter.Bounds.Width + 10;
            _tooltipPopover.VerticalOffset = -contentPresenter.Bounds.Height;
            _tooltipPopover.IsOpen = true;
            _tooltipPopover.Child.Opacity = 1;
            isOpen = true;
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            base.OnPointerLeave(e);
            if(_tooltipPopover?.Child == null) return;
            
            DispatcherTimer.RunOnce(() =>
            {
                if(!isOpen)
                    _tooltipPopover.IsOpen = false;
            }, TimeSpan.FromMilliseconds(100));
            
            isOpen = false;
            _tooltipPopover.Child.Opacity = 0;
        }
    }
}