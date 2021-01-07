using System;

namespace ChromER
{
    public class OpenDirectoryEventArgs : EventArgs
    {
        public FileEntityViewModel FileEntityViewModel { get; }

        public OpenDirectoryEventArgs(FileEntityViewModel fileEntityViewModel)
        {
            FileEntityViewModel = fileEntityViewModel;
        }
    }
}