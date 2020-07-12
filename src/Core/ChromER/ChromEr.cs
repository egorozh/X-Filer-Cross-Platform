namespace ChromER
{
    /// <summary>
    /// Основное приложение проводника chromER
    /// </summary>
    public class ChromEr
    {
        #region Singleton

        private static ChromEr _instance;

        public static ChromEr Instance => _instance ??= new ChromEr();

        #endregion

        #region Public Properties

        /// <summary>
        /// Менеджер иконок
        /// </summary>
        public IIconsManager IconsManager { get; }

        #endregion

        #region Constructor

        public ChromEr()
        {
            IconsManager = new IconsManager(new ExtensionToImageFileConverter());
        }

        #endregion
    }
}