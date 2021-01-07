using System;
using System.Collections.Generic;

namespace ChromER
{
    /// <summary>
    /// Основное приложение проводника chromER
    /// </summary>
    public sealed class ChromEr
    {
        private readonly ISynchronizationHelper _synchronizationHelper;
        private readonly Func<ITabClient> _tabClientFactory;

        #region Singleton

        public static ChromEr Instance { get; private set; }

        public static void CreateChromer(ISynchronizationHelper helper, Func<ITabClient> tabClientFactory)
        {
            if (Instance == null)
            {
                Instance = new ChromEr(helper, tabClientFactory);
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

        #endregion

        #region Constructor

        private ChromEr(ISynchronizationHelper synchronizationHelper, Func<ITabClient> tabClientFactoryFactory)
        {
            _synchronizationHelper = synchronizationHelper;
            _tabClientFactory = tabClientFactoryFactory;
            //MainViewModel = new MainViewModel(synchronizationHelper, _tabClientFactory.Invoke(),
            //    new[] {new DirectoryTabItemViewModel(_synchronizationHelper),});

            var converter = new ExtensionToImageFileConverter();

            IconsManager = new IconsManager(converter);
            BookmarksManager = new BookmarksManager(converter);
        }

        #endregion

        public MainViewModel CreateMainViewModel(IEnumerable<DirectoryTabItemViewModel> initItems)
            => new (_synchronizationHelper, _tabClientFactory?.Invoke(), initItems);
    }

    public interface ITabClient
    {
    }
}