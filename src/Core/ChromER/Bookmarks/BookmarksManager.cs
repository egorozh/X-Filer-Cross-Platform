using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ChromER
{
    internal class BookmarksManager : IBookmarksManager
    {
        #region Constants

        private const string BookmarksFileName = "bookmarks.json";

        #endregion

        #region Private Fields

        private readonly MainViewModel _mainViewModel;
        private readonly ExtensionToImageFileConverter _converter;

        private readonly ObservableCollection<MenuItemViewModel> _bookmarks;

        #endregion

        #region Public Properties

        public IReadOnlyCollection<MenuItemViewModel> Bookmarks => _bookmarks;

        #endregion

        #region Commands

        public DelegateCommand BookmarkClickCommand { get; }

        #endregion

        #region Constructor

        public BookmarksManager(MainViewModel mainViewModel, ExtensionToImageFileConverter converter)
        {
            _mainViewModel = mainViewModel;
            _converter = converter;
            BookmarkClickCommand = new DelegateCommand(OnBookmarkClicked);

            var items = OpenBookmarksFile();


            _bookmarks = CreateMenuItemViewModels(items);

            new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("C:\\")
                {
                    Header = "C:\\",
                    Command = BookmarkClickCommand
                },
                new MenuItemViewModel(@"C:\Games\ArtMoney")
                {
                    Header = @"ArtMoney",
                    Command = BookmarkClickCommand
                }
            };
        }

        private ObservableCollection<MenuItemViewModel> CreateMenuItemViewModels(IList<BookmarkItem> items)
        {
            var menuVms = new ObservableCollection<MenuItemViewModel>();

            if (items == null || !items.Any())
                return menuVms;

            foreach (var bookmarkItem in items)
            {
                var path = bookmarkItem.Path;

                var vm = new MenuItemViewModel(path)
                {
                    Items = CreateMenuItemViewModels(bookmarkItem.Children)
                };

                var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

                var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

                if (path == null)
                {
                    vm.Header = bookmarkItem.BookmarkFolderName;
                    vm.IconPath = Path.Combine(iconsDirectory.FullName, IconName.BookmarkFolder + ".svg");
                }
                else
                {
                    vm.Command = BookmarkClickCommand;

                    var attr = File.GetAttributes(path);

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        vm.Header = new DirectoryInfo(path).Name;
                        vm.IconPath = Path.Combine(iconsDirectory.FullName, IconName.Folder + ".svg");
                    }
                    else
                    {
                        var extension = new FileInfo(path).Extension;

                        vm.Header = new FileInfo(path).Name;
                        vm.IconPath =
                            _converter.GetImagePath(string.IsNullOrEmpty(extension) ? "" : extension.Substring(1))
                                .FullName;
                    }
                }


                menuVms.Add(vm);
            }

            return menuVms;
        }

        private List<BookmarkItem> OpenBookmarksFile()
        {
            if (File.Exists(BookmarksFileName))
            {
                var json = File.ReadAllText(BookmarksFileName);

                try
                {
                    return JsonSerializer.Deserialize<List<BookmarkItem>>(json);
                }
                catch (Exception e)
                {
                }
            }


            return new List<BookmarkItem>();
        }

        #endregion

        #region Commands Methods

        private void OnBookmarkClicked(object parameter)
        {
            if (parameter is string path)
                _mainViewModel.CurrentDirectoryTabItem.OpenBookmark(path);
        }

        #endregion
    }
}