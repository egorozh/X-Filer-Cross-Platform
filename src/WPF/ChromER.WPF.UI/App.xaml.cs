using Dragablz;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;

namespace ChromER.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WpfSynchronizationHelper synchronizationHelper = new();

            ChromEr.CreateChromer(synchronizationHelper, () => new BoundExampleInterTabClient());

            MainWindow mainWindow = new()
            {
                DataContext =
                    ChromEr.Instance.CreateMainViewModel(new DirectoryTabItemViewModel[] {new(synchronizationHelper),})
            };

            mainWindow.Show();

            //DebugWindow debugWindow = new();
            //debugWindow.Show();
        }
    }

    public class BoundExampleInterTabClient : IInterTabClient, ITabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            MainWindow view = new();
            var model = ChromEr.Instance.CreateMainViewModel(new List<DirectoryTabItemViewModel>());
            view.DataContext = model;
            return new NewTabHost<Window>(view, view.InitialTabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }

    public static class CloseActionStorage
    {
        public static ItemActionCallback ClosingTabItemHandler => ClosingTabItemHandlerImpl;

        private static void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model:
            var viewModel = args.DragablzItem.DataContext as DirectoryTabItemViewModel;

            //here's how you can cancel stuff:
            //args.Cancel(); 
        }
    }

    public class ChromerDragDrop : IDropTarget
    {
        private static IDropTarget _instance;

        public static IDropTarget Instance => _instance ??= new ChromerDragDrop();

        public void DragOver(IDropInfo dropInfo)
        {
            var debugWindow = Application.Current.Windows.OfType<DebugWindow>().FirstOrDefault();

            if (debugWindow != null)
            {
                debugWindow.TextBlock.Text =
                    $"Data: {dropInfo.Data}; TargetCollection: {dropInfo.TargetCollection}; TargetItem: {dropInfo.TargetItem}";
            }

            if (dropInfo.TargetCollection is ObservableCollection<FileEntityViewModel> targetCollection)
            {
                if (dropInfo.Data is FileEntityViewModel sourceItem)
                {
                    if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItem))
                    {
                        dropInfo.Effects = DragDropEffects.Move;
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                        dropInfo.EffectText = "Moveto";
                        dropInfo.DestinationText = "в папку DDD";
                    }

                    if (dropInfo.TargetItem is DirectoryViewModel targetFolder && targetFolder != sourceItem)
                    {
                        dropInfo.Effects = DragDropEffects.Move;
                        dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);
                        dropInfo.EffectText = "Moveto2";
                        dropInfo.DestinationText = "в папку DDD2";
                    }
                }

                if (dropInfo.Data is ICollection<object> sourceItems)
                {
                    if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItems.First()))
                    {
                        dropInfo.Effects = DragDropEffects.Move;
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                        return;
                    }

                    if (dropInfo.TargetItem is DirectoryViewModel directoryViewModel &&
                        !sourceItems.Contains(directoryViewModel))
                    {
                        dropInfo.Effects = DragDropEffects.Move;
                        dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);
                        return;
                    }
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
        }
    }

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