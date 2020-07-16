﻿using System;
using System.IO;
using ChromER.Shared.ViewModels;

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
            if (viewModel is DirectoryViewModel)
            {
                return _converter.GetImagePath(IconName.Folder);
            }
            else if (viewModel is FileViewModel fileViewModel)
            {
                var extension = new FileInfo(fileViewModel.FullName).Extension;

                var imagePath = _converter.GetImagePath(string.IsNullOrEmpty(extension) ? "" : extension.Substring(1));

                return imagePath;
            }

            throw new NotImplementedException();
        }
    }

    public interface IIconsManager
    {
        FileInfo GetIconPath(FileEntityViewModel viewModel);
    }
}