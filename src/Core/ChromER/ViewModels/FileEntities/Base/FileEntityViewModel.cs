namespace ChromER
{
    public abstract class FileEntityViewModel : BaseViewModel
    {
        public string Name { get; }

        public string FullName { get; set; }

        public abstract string ChangeDateTime { get; }

        protected FileEntityViewModel(string name)
        {
            Name = name;
        }

        public abstract string GetRootName();    
    }
}