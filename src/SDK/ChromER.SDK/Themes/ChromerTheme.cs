using Avalonia;
using System;
using PropertyChanged;

namespace ChromER.SDK
{
    [DoNotNotify]
    public abstract class ChromerTheme : AvaloniaObject
    {
        public string Guid => GetGuid();
        
        public abstract Uri GetResourceUri();

        public abstract string GetGuid();
    }
}   