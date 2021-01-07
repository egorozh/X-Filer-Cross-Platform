namespace ChromER
{
    public abstract class ChromerTabItemViewModel : BaseViewModel
    {
        #region Public Properties

        public string Header { get; set; }

        public bool IsSelected { get; set; }

        public object Content { get; set; }

        #endregion
    }
}