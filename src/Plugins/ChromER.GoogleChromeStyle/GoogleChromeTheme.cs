using ChromER.SDK;
using System;

namespace ChromER.GoogleChromeStyle
{
    public class GoogleChromeTheme : ChromerTheme
    {
        public override Uri GetResourceUri() =>
            new("avares://ChromER.GoogleChromeStyle/Themes/Generic.axaml",
                UriKind.Relative);

        public override string GetGuid() => "e394f339-5907-4c5f-9113-6e49368b3d22";
    }
}
