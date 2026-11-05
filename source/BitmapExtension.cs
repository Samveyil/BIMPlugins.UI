using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to load, scale and select frames of <see cref="BitmapSource"/> and <see cref="ImageSource"/>.
    /// </summary>
    public static class BitmapExtension
    {
        /// <summary>
        /// Creates a <see cref="BitmapFrame"/> from <paramref name="uriString"/>, choosing the largest frame that best matches the system DPI.
        /// </summary>
        /// <param name="uriString">Absolute or relative uri of the image.</param>
        /// <returns>The selected <see cref="BitmapFrame"/>.</returns>
        internal static BitmapFrame UriToBitmapFrame(string uriString)
        {
            var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
            var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.None, BitmapCacheOption.Default);
            return decoder.GetBitmapFrameByWidthAndDpi(int.MaxValue);
        }

        /// <summary>
        /// Transforms a base64 string or an uri into a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="base64orUri">
        /// Base64 image string, file path, url, pack uri or resource path.
        /// A resource path is resolved relative to the application, the calling assembly
        /// and the <c>Resources</c> folder of the calling assembly.
        /// </param>
        /// <returns>The loaded <see cref="BitmapSource"/>, or <c>null</c> if the image could not be loaded.</returns>
        public static BitmapSource GetBitmapSource(this string base64orUri)
        {
            try
            {
                return UriToBitmapFrame(base64orUri);
            }
            catch { }

            try
            {
                var componentUri = "pack://application:,,,/" + base64orUri.TrimStart('/');
                return UriToBitmapFrame(componentUri);
            }
            catch { }

            var executingAssembly = Utils.StackTraceUtils.GetCallingAssembly();
            var assemblyName = executingAssembly.GetName().Name;

            try
            {
                var componentUri = $"pack://application:,,,/{assemblyName};component/" + base64orUri.TrimStart('/');
                return UriToBitmapFrame(componentUri);
            }
            catch { }

            try
            {
                var componentUri = $"pack://application:,,,/{assemblyName};component/Resources/" + base64orUri.TrimStart('/');
                return UriToBitmapFrame(componentUri);
            }
            catch { }

            try
            {
                return Drawing.BitmapDrawingExtension.Base64ToBitmapSource(base64orUri);
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Scales <paramref name="bitmapSource"/> uniformly by <paramref name="scale"/>.
        /// </summary>
        /// <param name="bitmapSource">The bitmap to scale.</param>
        /// <param name="scale">Scale factor applied to both width and height.</param>
        /// <returns>A new <see cref="TransformedBitmap"/> with the scale applied.</returns>
        public static BitmapSource Scale(this BitmapSource bitmapSource, double scale)
        {
            return new TransformedBitmap(bitmapSource, new ScaleTransform(scale, scale));
        }
        /// <summary>
        /// Scales <paramref name="imageSource"/> uniformly by <paramref name="scale"/> if it is a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="imageSource">The image to scale.</param>
        /// <param name="scale">Scale factor applied to both width and height.</param>
        /// <returns>The scaled image, or <paramref name="imageSource"/> unchanged if it is not a <see cref="BitmapSource"/>.</returns>
        public static ImageSource Scale(this ImageSource imageSource, double scale)
        {
            if (imageSource is BitmapSource bitmapSource)
                return bitmapSource.Scale(scale);
            return imageSource;
        }

        /// <summary>
        /// Gets the system DPI based on <see cref="VisualTreeHelper.GetDpi"/> using a new <see cref="System.Windows.Controls.Image"/>.
        /// </summary>
        /// <returns>The system DPI, or 96 if it could not be determined.</returns>
        internal static double GetSystemDpi()
        {
            double systemDpi = 96;
            try
            {
                var imageScaleInfo = VisualTreeHelper.GetDpi(new System.Windows.Controls.Image());
                systemDpi = imageScaleInfo.PixelsPerInchX;
            }
            catch { }
            return systemDpi;
        }

        /// <summary>
        /// System DPI, calculated once when the class is first used.
        /// </summary>
        public readonly static double SystemDpi = GetSystemDpi();

        /// <summary>
        /// Gets the frame from <paramref name="bitmapDecoder"/> that best matches the DPI and <paramref name="width"/>.
        /// </summary>
        /// <param name="bitmapDecoder">The bitmap decoder.</param>
        /// <param name="width">The desired frame width. When greater than the largest frame, the largest frame is used; when zero, the smallest frame is returned.</param>
        /// <param name="dpi">The optimal DPI for the frame. When zero, <see cref="SystemDpi"/> is used.</param>
        /// <returns>The first frame, ordered by DPI match, with width equal to or greater than <paramref name="width"/>.</returns>
        public static BitmapFrame GetBitmapFrameByWidthAndDpi(this BitmapDecoder bitmapDecoder, int width, int dpi = 0)
        {
            double systemDpi = dpi > 0 ? dpi : SystemDpi;

            double OrderDpiX(BitmapFrame frame)
            {
                var dpiX = Math.Round(frame.DpiX);
                return dpiX >= systemDpi ? -systemDpi / dpiX : systemDpi / dpiX;
            }

            var frames = bitmapDecoder.Frames;
            var framesOrder = frames
                .OrderBy(OrderDpiX)
                .ThenBy(e => Math.Round(e.Width));

            var widthMax = (int)Math.Round(framesOrder.LastOrDefault().Width);
            if (width > widthMax)
                width = widthMax;

            var frame = framesOrder.FirstOrDefault(e => Math.Round(e.Width) >= width);
            return frame;
        }

        /// <summary>
        /// Gets the frame of <paramref name="imageSource"/> with the given <paramref name="width"/>,
        /// scaling the image down when it is still wider than <paramref name="width"/>.
        /// </summary>
        /// <typeparam name="TImageSource">Type of the image source.</typeparam>
        /// <param name="imageSource">The image source.</param>
        /// <param name="width">The desired frame width. When zero, the smallest frame is returned and no scaling is applied.</param>
        /// <param name="downloadCompleted">Optional action invoked with the final image when a <see cref="BitmapFrame"/> that is still downloading completes.</param>
        /// <returns>The frame with the specified width, or the scaled frame.</returns>
        public static TImageSource GetBitmapFrame<TImageSource>(this TImageSource imageSource, int width = 0, Action<TImageSource> downloadCompleted = null) where TImageSource : ImageSource
        {
            TImageSource ScaleDownIfWidthIsGreater(TImageSource imageSource, int width)
            {
                if (width <= 0)
                    return imageSource;

                var imageRoundWidth = Math.Round(imageSource.Width);
                if (imageRoundWidth > width)
                    imageSource = imageSource.Scale(width / imageRoundWidth) as TImageSource;

                return imageSource;
            }

            if (imageSource is BitmapFrame bitmapFrame)
            {
                if (bitmapFrame.IsDownloading)
                {
                    bitmapFrame.DownloadCompleted += (s, e) =>
                    {
                        if (bitmapFrame.Decoder.GetBitmapFrameByWidthAndDpi(width) is TImageSource frame)
                            imageSource = frame;

                        imageSource = ScaleDownIfWidthIsGreater(imageSource, width);

                        downloadCompleted?.Invoke(imageSource);
                    };
                }

                if (bitmapFrame.Decoder.GetBitmapFrameByWidthAndDpi(width) is TImageSource frame)
                    imageSource = frame;
            }

            imageSource = ScaleDownIfWidthIsGreater(imageSource, width);

            return imageSource;
        }
    }
}