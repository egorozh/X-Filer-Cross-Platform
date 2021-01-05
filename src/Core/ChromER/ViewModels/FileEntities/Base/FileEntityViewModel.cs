namespace ChromER
{
    public abstract class FileEntityViewModel : BaseViewModel
    {
        public string Name { get; }

        public string FullName { get; set; }

        protected FileEntityViewModel(string name)
        {
            Name = name;
        }

        public abstract string GetRootName();    
    }
}