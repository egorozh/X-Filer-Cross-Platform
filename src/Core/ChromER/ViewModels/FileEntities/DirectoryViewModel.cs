using System.IO;

namespace ChromER
{
    public class DirectoryViewModel : FileEntityViewModel
    {
        public DirectoryViewModel(DirectoryInfo directoryName) : base(directoryName.Name)
        {
            FullName = directoryName.FullName;
        }

        public override string GetRootName()
            => new DirectoryInfo(FullName).Root.Name;
    }
}