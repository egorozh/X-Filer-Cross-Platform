using System;
using Avalonia.Media.Imaging;

namespace ChromER.SDK
{
    public abstract class FileEntityViewModel : BaseViewModel
    {
        protected IIconLoader IconLoader { get; }

        #region Public Properties

        public string Name { get; set; }

        public string FullName { get; set; }

        public string? Group { get; set; }

        public abstract DateTime ChangeDateTime { get; }

        public Bitmap? Icon { get; set; }

        #endregion

        #region Constructor

        protected FileEntityViewModel(string name, string fullName, IIconLoader iconLoader)
        {
            Name = name;
            FullName = fullName;
            IconLoader = iconLoader;

            Icon = iconLoader.GetIcon(this);
        }

        #endregion


        public abstract string? GetRootName();

        public abstract FileEntityViewModel Clone();
    }
}