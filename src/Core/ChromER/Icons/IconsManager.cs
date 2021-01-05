﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ChromER
{
    internal class IconsManager : IIconsManager
    {
        private readonly ExtensionToImageFileConverter _converter;

        public IconsManager(ExtensionToImageFileConverter converter)
        {
            _converter = converter;
        }

        public FileInfo GetIconPath(FileEntityViewModel viewModel)
        {
            if (viewModel is FileViewModel fileViewModel)
            {
                var extension = new FileInfo(fileViewModel.FullName).Extension;

                var imagePath = _converter.GetImagePath(string.IsNullOrEmpty(extension) ? "" : extension.Substring(1));

                return imagePath;
            }

            if (viewModel is LogicalDriveViewModel logicalDriveViewModel)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (logicalDriveViewModel.FullName == "C:\\")
                        return _converter.GetImagePath(IconName.SystemDrive);
                }

                return _converter.GetImagePath(IconName.LogicalDrive);
            }

            if (viewModel is DirectoryViewModel)
            {
                return _converter.GetImagePath(IconName.Folder);
            }

            throw new NotImplementedException();
        }
    }

    public interface IIconsManager
    {
        FileInfo GetIconPath(FileEntityViewModel viewModel);
    }
}