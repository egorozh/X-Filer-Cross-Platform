using System;

namespace ChromER
{
    public abstract class FileEntityViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public abstract DateTime ChangeDateTime { get; }

        protected FileEntityViewModel(string name)
        {
            Name = name;
        }

        public abstract string GetRootName();    
    }
}