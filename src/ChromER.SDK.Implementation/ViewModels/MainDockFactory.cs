using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;

namespace ChromER.SDK
{
    public class MainDockFactory : Factory
    {
        private readonly IFilesPresenterFactory _filesPresenterFactory;
        private readonly IBookmarksManager _bookmarksManager;
        private readonly IExplorerTabFactory _explorerTabFactory;

        private IDock _documentDock;
        private object _context = new object();

        public MainDockFactory(IFilesPresenterFactory filesPresenterFactory,
            IBookmarksManager bookmarksManager,
            IExplorerTabFactory explorerTabFactory)
        {
            _filesPresenterFactory = filesPresenterFactory;
            _bookmarksManager = bookmarksManager;
            _explorerTabFactory = explorerTabFactory;
        }

        public override IRootDock CreateLayout()
        {
            var document1 = CreateExplorerTabItemViewModel(new DirectoryInfo("C:\\"));
            document1.Id = "C:\\";
            document1.Title = "Локальный диск C";

            var document2 = CreateExplorerTabItemViewModel(new DirectoryInfo("D:\\"));
            document2.Id = "D:\\";
            document2.Title = "Локальный диск D";

            var documentDock = new DocumentDock
            {
                Id = "DocumentsPane",
                Title = "DocumentsPane",
                Proportion = double.NaN,
                ActiveDockable = document1,
                VisibleDockables = new ObservableCollection<IDockable>(new[]
                {
                    document1,
                    document2
                }),
                CanCreateDocument = true,
               
            };

            documentDock.CreateDocument = ReactiveCommand.Create(() =>
            {
                var index = documentDock.VisibleDockables?.Count + 1;
                var document = _explorerTabFactory.CreateRootTab();
                document.Id = $"IChromerApp.RootName";
                document.Title = $"Мой компьютер";

                this.AddDockable(documentDock, document);
                this.SetActiveDockable(document);
                this.SetFocusedDockable(documentDock, document);
            });

            var mainLayout = new ProportionalDock
            {
                Id = "MainLayout",
                Title = "MainLayout",
                Proportion = double.NaN,
                Orientation = Orientation.Horizontal,
                ActiveDockable = null,
                VisibleDockables = new ObservableCollection<IDockable>(new IDockable[]
                {
                    documentDock
                })
            };

            var mainView = new MainViewModel
            {
                Id = "Main",
                Title = "Main",
                ActiveDockable = mainLayout,
                VisibleDockables = new ObservableCollection<IDockable>(new[] {mainLayout})
            };

            var root = new RootDock
            {
                Id = "Root",
                Title = "Root",
                ActiveDockable = mainView,
                DefaultDockable = mainView,
                VisibleDockables = new ObservableCollection<IDockable>(new[] {mainView})
            };

            _documentDock = documentDock;

            return root;
        }

        public override void InitLayout(IDockable layout)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                [nameof(IRootDock)] = () => _context,
                [nameof(IProportionalDock)] = () => _context,
                [nameof(IDocumentDock)] = () => _context,
                [nameof(IToolDock)] = () => _context,
                [nameof(IDockWindow)] = () => _context,
                [nameof(IDocument)] = () => _context,
                [nameof(ITool)] = () => _context,
                ["C:\\"] = () => CreateExplorerTabItemViewModel(new DirectoryInfo("C:\\")),
                ["D:\\"] = () => CreateExplorerTabItemViewModel(new DirectoryInfo("D:\\")),

                ["LeftPane"] = () => _context,
                ["LeftPaneTop"] = () => _context,
                ["LeftPaneTopSplitter"] = () => _context,
                ["LeftPaneBottom"] = () => _context,
                ["RightPane"] = () => _context,
                ["RightPaneTop"] = () => _context,
                ["RightPaneTopSplitter"] = () => _context,
                ["RightPaneBottom"] = () => _context,
                ["DocumentsPane"] = () => _context,
                ["MainLayout"] = () => _context,
                ["LeftSplitter"] = () => _context,
                ["RightSplitter"] = () => _context,
                ["MainLayout"] = () => _context,
                ["Main"] = () => _context,
            };

            this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = () =>
                {
                    var hostWindow = new HostWindow()
                    {
                        [!Window.TitleProperty] = new Binding("ActiveDockable.Title")
                    };
                    return hostWindow;
                }
            };

            this.DockableLocator = new Dictionary<string, Func<IDockable>>
            {
            };

            base.InitLayout(layout);

            this.SetActiveDockable(_documentDock);
            this.SetFocusedDockable(_documentDock, _documentDock.VisibleDockables?.FirstOrDefault());
        }

        private ExplorerTabItemViewModel CreateExplorerTabItemViewModel(DirectoryInfo directoryInfo)
        {
            var item = new ExplorerTabItemViewModel();

            item.Init(_filesPresenterFactory, _bookmarksManager, directoryInfo);

            return item;
        }

        public void InitLayoutAfterDeserialize(IRootDock layout)
        {
            _documentDock = SearchDocumentDock(layout);

            foreach (var visibleDockable in _documentDock.VisibleDockables)
            {
                if (visibleDockable is IDocumentDock 
                    {ActiveDockable: ExplorerTabItemViewModel explorerVm})
                {
                    explorerVm.Init(_filesPresenterFactory, _bookmarksManager, 
                        new DirectoryInfo(explorerVm.Id));
                }
            }

            InitLayout(layout);
        }
        
        private IDock? SearchDocumentDock(IDock layout)
        {
            if (layout.VisibleDockables.Any(d => d is IDocumentDock documentDock))
                return layout as IDock;

            foreach (var visibleDockable in layout.VisibleDockables)
            {
                var d = SearchDocumentDock((IDock) visibleDockable);

                if (d != null)
                    return d;
            }

            return null;
        }
    }
}