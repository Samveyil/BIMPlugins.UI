using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Helpers to switch between light and dark image variants.
    /// An image takes part in theme switching when its source name contains <c>light</c> or <c>dark</c>,
    /// for example <c>Resources/icon-light.png</c> and <c>Resources/icon-dark.png</c>.
    /// </summary>
    public static class RibbonThemeImageUtils
    {
        private const string NAME_DARK = "dark";
        private const string NAME_LIGHT = "light";

        /// <summary>
        /// Gets the variant of <paramref name="imageSource"/> for the light or dark theme.
        /// </summary>
        /// <typeparam name="TImageSource">Type of the image source.</typeparam>
        /// <param name="imageSource">The image source.</param>
        /// <param name="isLight"><c>true</c> to get the variant for the light theme; <c>false</c> for the dark theme.</param>
        /// <returns>The image for the requested theme, or <paramref name="imageSource"/> if no such variant exists.</returns>
        public static TImageSource GetThemeImageSource<TImageSource>(this TImageSource imageSource, bool isLight = true) where TImageSource : ImageSource
        {
            if (imageSource.GetSourceName().TryThemeImage(isLight, out string imageTheme))
            {
                if (imageTheme.GetBitmapSource() is TImageSource themeImageSource)
                    return themeImageSource;
            }

            return imageSource;
        }

        /// <summary>
        /// Tries to build the image path for the requested theme by replacing <c>light</c> with <c>dark</c>, or the opposite, in <paramref name="image"/>.
        /// For a pack uri only the last component is changed.
        /// </summary>
        /// <param name="image">The image path or uri.</param>
        /// <param name="isLight"><c>true</c> for the light theme; <c>false</c> for the dark theme.</param>
        /// <param name="imageTheme">The image path for the requested theme, in lower case.</param>
        /// <returns><c>true</c> if <paramref name="image"/> contains the opposite theme name and <paramref name="imageTheme"/> was built.</returns>
        internal static bool TryThemeImage(this string image, bool isLight, out string imageTheme)
        {
            imageTheme = string.Empty;

            if (string.IsNullOrEmpty(image))
                return false;

            var findThemeName = !isLight ? NAME_LIGHT : NAME_DARK;
            var replaceThemeName = isLight ? NAME_LIGHT : NAME_DARK;

            if (image.IndexOf(findThemeName, System.StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                image = image.ToLowerInvariant();

                if (image.StartsWith("pack://application:,,,/"))
                {
                    var components = image.Split(';');
                    components[components.Length - 1] = components[components.Length - 1].Replace(findThemeName, replaceThemeName);

                    imageTheme = string.Join(";", components);
                    return true;
                }

                imageTheme = image.Replace(findThemeName, replaceThemeName);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Gets the source uri of <paramref name="imageSource"/> if it is a <see cref="TransformedBitmap"/> or <see cref="BitmapFrame"/>.
        /// </summary>
        /// <param name="imageSource">The image source.</param>
        /// <returns>The source uri in lower case, or <c>null</c>.</returns>
        internal static string GetSourceName(this ImageSource imageSource)
        {
            if (imageSource is TransformedBitmap transformedBitmap)
            {
                return transformedBitmap.Source.ToString().ToLowerInvariant();
            }
            if (imageSource is BitmapFrame bitmapFrame)
            {
                return bitmapFrame.Decoder.ToString().ToLowerInvariant();
            }
            return null;
        }
    }
}