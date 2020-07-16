namespace ChromER
{
    /// <summary>
    /// Основное приложение проводника chromER
    /// </summary>
    public sealed class ChromEr
    {
        #region Singleton

        public static ChromEr Instance { get; private set; }

        public static void CreateChromer(ISynchronizationHelper helper)
        {
            if (Instance == null)
            {
                Instance = new ChromEr(helper);
            }
        }

        #endregion

        #region Public Properties

        public MainViewModel MainViewModel { get; }

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

        private ChromEr(ISynchronizationHelper synchronizationHelper)
        {
            MainViewModel = new MainViewModel(synchronizationHelper);

            var converter = new ExtensionToImageFileConverter();

            IconsManager = new IconsManager(converter);
            BookmarksManager = new BookmarksManager(MainViewModel, converter);
        }

        #endregion
    }
}