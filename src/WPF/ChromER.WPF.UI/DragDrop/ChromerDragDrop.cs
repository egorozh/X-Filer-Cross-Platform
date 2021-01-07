using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChromER.WPF.UI
{
    public class ChromerDragDrop : IDropTarget
    {
        #region Singleton

        private static IDropTarget _instance;

        public static IDropTarget Instance => _instance ??= new ChromerDragDrop();

        #endregion

        #region Public Methods

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo?.DragInfo == null)
            {
                return;
            }

            if (!dropInfo.IsSameDragDropContextAsSource)
            {
                return;
            }

            //var debugWindow = Application.Current.Windows.OfType<DebugWindow>().FirstOrDefault();

            //if (debugWindow != null)
            //{
            //    debugWindow.TextBlock.Text =
            //        $"{dropInfo.VisualTarget} ; {dropInfo.VisualTargetItem}";
            //}

            if (dropInfo.TargetCollection is ObservableCollection<FileEntityViewModel> targetCollection)
            {
                if (CanDrag(dropInfo, targetCollection))
                    return;
            }

            dropInfo.EffectText = null;
            dropInfo.DestinationText = null;
            dropInfo.DropTargetAdorner = null;
            dropInfo.Effects = DragDropEffects.Scroll;
        }

        public void Drop(IDropInfo dropInfo)
        {
        }

        #endregion

        #region Private Methods

        private bool CanDrag(IDropInfo dropInfo, ObservableCollection<FileEntityViewModel> targetCollection)
            => dropInfo.Data switch
            {
                FileEntityViewModel sourceItem => CanDragOneSourceItem(dropInfo, targetCollection, sourceItem),
                ICollection<object> sourceItems => CanDragManySourceItems(dropInfo, targetCollection, sourceItems),
                _ => false
            };

        private bool CanDragOneSourceItem(IDropInfo dropInfo,
            ObservableCollection<FileEntityViewModel> targetCollection,
            FileEntityViewModel sourceItem)
        {
            var sourceRoot = sourceItem.GetRootName();

            var debugWindow = Application.Current.Windows.OfType<DebugWindow>().FirstOrDefault();

            if (debugWindow != null)
            {
                debugWindow.TextBlock.Text =
                    $"{dropInfo.TargetItem}";
            }

            if (dropInfo.TargetItem is DirectoryViewModel targetFolder && targetFolder != sourceItem)
            {
                var targetRoot = targetFolder.GetRootName();

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetFolder.Name}";
                }
                else if (sourceRoot == targetRoot)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                    dropInfo.EffectText = "Переместить в";
                    dropInfo.DestinationText = $"{targetFolder.Name}";
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    dropInfo.EffectText = "Копировать в";
                    dropInfo.DestinationText = $"{targetFolder.Name}";
                }

                return true;
            }

            if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItem))
            {
                var targetVisual = (ItemsControl) dropInfo.VisualTarget;
                var viewModel = targetVisual.DataContext as IFilesPresenter;
                DirectoryInfo targetDirectory = new(viewModel.CurrentDirectoryPathName);

                var targetRoot = targetDirectory.Root.Name;

                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else if (sourceRoot == targetRoot)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                    dropInfo.EffectText = "Переместить в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    dropInfo.EffectText = "Копировать в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }

                return true;
            }

            return false;
        }

        private bool CanDragManySourceItems(IDropInfo dropInfo,
            ObservableCollection<FileEntityViewModel> targetCollection,
            ICollection<object> sourceItems)
        {
            if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItems.First()))
            {
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                return true;
            }

            if (dropInfo.TargetItem is DirectoryViewModel directoryViewModel &&
                !sourceItems.Contains(directoryViewModel))
            {
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);
                return true;
            }

            return false;
        }

        #endregion
    }
}