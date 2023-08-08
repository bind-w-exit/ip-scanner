using Windows.UI.Xaml;

namespace IpScanner.Ui.Extensions
{
    public static class ElementThemeExtensions
    {
        public static ElementTheme ToElementTheme(this string themeString)
        {
            switch (themeString?.Trim().ToLower())
            {
                case "light":
                    return ElementTheme.Light;
                case "dark":
                    return ElementTheme.Dark;
                default:
                    return ElementTheme.Default;
            }
        }
    }
}
