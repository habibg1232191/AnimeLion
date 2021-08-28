using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AnimeLion.Views.Controls
{
    public class ScrollBar : Panel
    {
        public ScrollBar()
        {
            AddHandler(PointerWheelChangedEvent, OnPointerWheelChanged,
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        }
        
        private double scrollBarOffsetX = 0;
	private bool isFirst = true;

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
	    
	    if(isFirst){
	        if(e.Delta.Y > 0)
                {
                    if(scrollBarOffsetX + 1 < Children.Count)
                        scrollBarOffsetX += 1;
                }
                else if (e.Delta.Y < 0)
                {
                    if(scrollBarOffsetX - 1 >= 0)
                        scrollBarOffsetX -= 1;
                }

                InvalidateArrange();
            
                Console.WriteLine(scrollBarOffsetX);
		isFirst = false;
	    }else{
		isFirst = true;
	    }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size stackDesiredSize = new Size();
            Size layoutSlotSize = availableSize;
            
            layoutSlotSize = layoutSlotSize.WithWidth(Double.PositiveInfinity);

            var children = Children;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null || !child.IsVisible) continue;
                
                child.Measure(layoutSlotSize);
                Size childDesiredSize = child.DesiredSize;
                
                stackDesiredSize = stackDesiredSize.WithWidth(stackDesiredSize.Width + childDesiredSize.Width);
                stackDesiredSize = stackDesiredSize.WithHeight(Math.Max(stackDesiredSize.Height, childDesiredSize.Height));
            }
            
            stackDesiredSize = stackDesiredSize.WithWidth(stackDesiredSize.Width);
            
            return stackDesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = Children;
            Rect rcChild = new Rect(finalSize);
            double previousChildSize = 0.0;
            double leftSpacing = 0;
            double xPos = 0;
            bool isFirst = false;
            
            if(scrollBarOffsetX > 0 & scrollBarOffsetX <= children.Count - 1)
                for (int i = 0; i <= scrollBarOffsetX-1; i++)
                    leftSpacing += children[i].DesiredSize.Width;

            xPos = -leftSpacing;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null || !child.IsVisible) continue;
                
                rcChild = rcChild.WithX(xPos);
                
                xPos += child.DesiredSize.Width;
                
                previousChildSize = child.DesiredSize.Width;
                rcChild = rcChild.WithWidth(previousChildSize);
                rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                
                ArrangeChild(child, rcChild);
            }
            
            return finalSize;
        }
        
        private void ArrangeChild(IControl child, Rect rect)
        {
            child.Arrange(rect);
        }
    }
}