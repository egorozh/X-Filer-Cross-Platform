using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;

namespace ChromER.WPF.UI
{
    public class ChromerDropTargetHighlightAdorner : DropTargetAdorner
    {
        public ChromerDropTargetHighlightAdorner(UIElement adornedElement, DropInfo dropInfo)
            : base(adornedElement, dropInfo)
        {
            Pen = new Pen(Brushes.DeepSkyBlue, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var dropInfo = this.DropInfo;
            var visualTargetItem = dropInfo.VisualTargetItem;
            if (visualTargetItem != null)
            {
                var rect = Rect.Empty;

                if (visualTargetItem is TreeViewItem tvItem && VisualTreeHelper.GetChildrenCount(tvItem) > 0)
                {
                    var descendant = VisualTreeHelper.GetDescendantBounds(tvItem);
                    var translatePoint = tvItem.TranslatePoint(new Point(), this.AdornedElement);
                    var itemRect = new Rect(translatePoint, tvItem.RenderSize);
                    descendant.Union(itemRect);
                    translatePoint.Offset(1, 0);
                    rect = new Rect(translatePoint,
                        new Size(descendant.Width - translatePoint.X - 1, tvItem.ActualHeight));
                }

                if (visualTargetItem is ListBoxItem listBoxItem)
                {
                    rect = new Rect(visualTargetItem.TranslatePoint(new Point(), this.AdornedElement),
                        new Size(listBoxItem.ActualWidth, listBoxItem.ActualHeight));
                }

                if (rect.IsEmpty)
                {
                    rect = new Rect(visualTargetItem.TranslatePoint(new Point(), this.AdornedElement),
                        VisualTreeHelper.GetDescendantBounds(visualTargetItem).Size);
                }

                drawingContext.DrawRoundedRectangle(null, this.Pen, rect, 2, 2);
            }
        }
    }
}