using System.IO;

namespace ChromER
{
    public class DirectoryViewModel : FileEntityViewModel
    {
        private readonly DirectoryInfo _directoryName;

        public DirectoryViewModel(DirectoryInfo directoryName) : base(directoryName.Name)
        {
            _directoryName = directoryName;
            FullName = directoryName.FullName;
        }

        public override string ChangeDateTime => _directoryName.LastWriteTime.ToString();

        public override string GetRootName()
            => new DirectoryInfo(FullName).Root.Name;
    }
}