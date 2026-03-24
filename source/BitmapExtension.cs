using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BIMPlugins.UI
{
    public static class BitmapExtension
    {
        internal static BitmapFrame UriToBitmapFrame(string uriString)
        {
            var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
            var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.None, BitmapCacheOption.Default);
            return decoder.GetBitmapFrameByWidthAndDpi(int.MaxValue);
        }

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

            return null;
        }
        public static BitmapSource GetBitmapSource(this string Uri, string assemblyName)
        {
            try
            {
                var componentUri = $"pack://application:,,,/{assemblyName};component/" + Uri.TrimStart('/');
                return UriToBitmapFrame(componentUri);
            }
            catch { }

            return null;
        }

        public static BitmapSource Scale(this BitmapSource bitmapSource, double scale)
        {
            return new TransformedBitmap(bitmapSource, new ScaleTransform(scale, scale));
        }
        public static ImageSource Scale(this ImageSource imageSource, double scale)
        {
            if (imageSource is BitmapSource bitmapSource)
                return bitmapSource.Scale(scale);
            return imageSource;
        }

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

        public readonly static double SystemDpi = GetSystemDpi();

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