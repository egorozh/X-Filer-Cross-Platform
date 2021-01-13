using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace ChromER
{
    /// <summary>
    /// Основное приложение проводника chromER
    /// </summary>
    public sealed class ChromEr
    {
        private readonly ISynchronizationHelper _synchronizationHelper;
        private readonly ITabClient _tabClient;
        private readonly Action<MainViewModel, Point> _windowFactory;

        #region Singleton

        public static ChromEr? Instance { get; private set; }

        public static void CreateChromer(ISynchronizationHelper helper, ITabClient tabClient,
            Action<MainViewModel, Point> windowFactory)
        {
            if (Instance == null)
            {
                CultureInfo currentCulture = new("Ru-ru");
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentCulture;

                Instance = new ChromEr(helper, tabClient, windowFactory);
            }
        }

        public static string RootName = "Мой компьютер";

        #endregion

        #region Public Properties

        //public MainViewModel MainViewModel { get; }

        /// <summary>
        /// Менеджер иконок
        /// </summary>
        public IIconsManager IconsManager { get; }

        /// <summary>
        /// Менеджер закладок
        /// </summary>
        public IBookmarksManager BookmarksManager { get; }

        public DelegateCommand OpenNewWindowCommand { get; }

        #endregion

        #region Constructor

        private ChromEr(ISynchronizationHelper synchronizationHelper,
            ITabClient tabClient,
            Action<MainViewModel, Point> windowFactory)
        {
            _synchronizationHelper = synchronizationHelper;
            _tabClient = tabClient;
            _windowFactory = windowFactory;

            var converter = new ExtensionToImageFileConverter();

            OpenNewWindowCommand = new DelegateCommand(OnOpenNewWindow);

            IconsManager = new IconsManager(converter);
            BookmarksManager = new BookmarksManager(converter);
        }

        #endregion

        public MainViewModel CreateMainViewModel(IEnumerable<DirectoryTabItemViewModel> initItems)
            => new(_synchronizationHelper, _tabClient, initItems);

        private void OnOpenNewWindow(object parameter)
        {
            if (parameter is FileEntityViewModel fileEntityViewModel)
            {
                if (fileEntityViewModel is DirectoryViewModel directoryViewModel)
                {
                    var mainViewModel = CreateMainViewModel(new DirectoryTabItemViewModel[0]);

                    var myCompTabVm = new DirectoryTabItemViewModel(
                        _synchronizationHelper, 
                        directoryViewModel.FullName,
                        directoryViewModel.Name);

                    mainViewModel.TabItems.Add(myCompTabVm);

                    _windowFactory.Invoke(mainViewModel, new Point(24, 24));
                }
            }
        }
    }

    public interface ITabClient
    {
    }
}