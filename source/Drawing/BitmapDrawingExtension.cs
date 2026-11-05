using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace BIMPlugins.UI.Drawing
{
    /// <summary>
    /// Extension methods to convert <c>System.Drawing</c> images to WPF <see cref="BitmapSource"/>.
    /// </summary>
    public static class BitmapDrawingExtension
    {
        /// <summary>
        /// Converts <paramref name="bitmap"/> to a <see cref="BitmapSource"/> with 96 DPI.
        /// </summary>
        /// <param name="bitmap">The bitmap to convert.</param>
        /// <returns>The converted <see cref="BitmapSource"/>.</returns>
        public static BitmapSource GetBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            var data = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                data.Width, data.Height, 96.0, 96.0,
                System.Windows.Media.PixelFormats.Bgra32, null,
                data.Scan0, data.Stride * data.Height, data.Stride);

            bitmap.UnlockBits(data);

            return bitmapSource;
        }
        /// <summary>
        /// Converts <paramref name="icon"/> to a <see cref="BitmapSource"/>, using its largest frame.
        /// </summary>
        /// <param name="icon">The icon to convert.</param>
        /// <returns>The largest frame of the icon.</returns>
        public static BitmapSource GetBitmapSource(this System.Drawing.Icon icon)
        {
            var stream = new MemoryStream();
            icon.Save(stream);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
            return decoder.Frames.OrderBy(e => e.Width).LastOrDefault();
        }
        /// <summary>
        /// Converts <paramref name="image"/> to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>The converted <see cref="BitmapSource"/>.</returns>
        public static BitmapSource GetBitmapSource(this System.Drawing.Image image)
        {
            var bitmap = new System.Drawing.Bitmap(image);
            return bitmap.GetBitmapSource();
        }

        /// <summary>
        /// Converts a base64 encoded image to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="base64">The base64 encoded image.</param>
        /// <returns>The converted <see cref="BitmapSource"/>.</returns>
        /// <exception cref="FormatException"><paramref name="base64"/> is not a valid base64 string.</exception>
        internal static BitmapSource Base64ToBitmapSource(string base64)
        {
            var convert = Convert.FromBase64String(base64);
            var image = System.Drawing.Bitmap.FromStream(new MemoryStream(convert));
            return image.GetBitmapSource();
        }
    }
}