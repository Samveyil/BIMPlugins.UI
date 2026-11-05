using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;
using System.Windows.Media;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Fluent extension methods to configure a <see cref="RibbonItem"/> already added to the ribbon.
    /// </summary>
    public static class RibbonItemExtension
    {
        /// <summary>
        /// Sets the contextual help (F1) of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="helpPath">An url (starting with <c>http</c>) or a path to a CHM file. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetContextualHelp<TRibbonItem>(this TRibbonItem ribbonItem, string helpPath) where TRibbonItem : RibbonItem
        {
            if (helpPath != null)
                ribbonItem.SetContextualHelp(RibbonHelpExtension.GetContextualHelp(helpPath));

            return ribbonItem;
        }

        /// <summary>
        /// Shows or hides the text of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="showText"><c>true</c> to show the text; <c>false</c> to hide it.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetShowText<TRibbonItem>(this TRibbonItem ribbonItem, bool showText = false) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().ShowText = showText;
            return ribbonItem;
        }

        /// <summary>
        /// Shows or hides the image of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="showImage"><c>true</c> to show the image; <c>false</c> to hide it.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetShowImage<TRibbonItem>(this TRibbonItem ribbonItem, bool showImage = false) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().ShowImage = showImage;
            return ribbonItem;
        }

        /// <summary>
        /// Sets the display size of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="itemSize">The item size.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetItemSize<TRibbonItem>(this TRibbonItem ribbonItem, Autodesk.Windows.RibbonItemSize itemSize = Autodesk.Windows.RibbonItemSize.Large) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().Size = itemSize;
            return ribbonItem;
        }

        /// <summary>
        /// Sets the text of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="text">The text. When <c>null</c>, nothing is changed; when empty or whitespace, the text is hidden.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetText<TRibbonItem>(this TRibbonItem ribbonItem, string text = "") where TRibbonItem : RibbonItem
        {
            if (text == null)
                return ribbonItem;

            if (text.Trim() == string.Empty)
                return ribbonItem.SetShowText(false);

            ribbonItem.ItemText = text;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="toolTip">The tooltip text. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTip<TRibbonItem>(this TRibbonItem ribbonItem, string toolTip) where TRibbonItem : RibbonItem
        {
            if (toolTip != null)
                ribbonItem.ToolTip = toolTip;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the long description shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="longDescription">The long description. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLongDescription<TRibbonItem>(this TRibbonItem ribbonItem, string longDescription) where TRibbonItem : RibbonItem
        {
            if (longDescription != null)
                ribbonItem.LongDescription = longDescription;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the image shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="toolTipImage">The tooltip image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource toolTipImage) where TRibbonItem : RibbonItem
        {
            ribbonItem.ToolTipImage = toolTipImage;
            return ribbonItem;
        }
        /// <summary>
        /// Sets the image shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="toolTipImage">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string toolTipImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = toolTipImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(toolTipImage))
                return ribbonItem;

            return ribbonItem.SetToolTipImage(bitmapSource);
        }

        /// <summary>
        /// Sets the small (16 px) image of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="image">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string image) where TRibbonItem : RibbonItem
        {
            var bitmapSource = image?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(image))
                return ribbonItem;

            return ribbonItem.SetImage(bitmapSource);
        }
        /// <summary>
        /// Sets the large (32 px) image of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="largeImage">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string largeImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = largeImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(largeImage))
                return ribbonItem;

            return ribbonItem.SetLargeImage(bitmapSource);
        }

        /// <summary>
        /// Sets the small (16 px) image of <paramref name="ribbonItem"/>, using the image variant for the current Revit theme.
        /// Supports <see cref="RibbonButton"/>, <see cref="ComboBox"/>, <see cref="ComboBoxMember"/> and <see cref="TextBox"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="image">The image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource image) where TRibbonItem : RibbonItem
        {
            image = image.GetThemeImageSource(RibbonThemeUtils.IsLight);

            if (ribbonItem is RibbonButton ribbonButton)
                ribbonButton.Image = image?.GetBitmapFrame(16, (frame) => { ribbonButton.Image = frame; });

            else if (ribbonItem is ComboBox comboBox)
                comboBox.Image = image?.GetBitmapFrame(16, (frame) => { comboBox.Image = frame; });

            else if (ribbonItem is ComboBoxMember comboBoxMember)
                comboBoxMember.Image = image?.GetBitmapFrame(16, (frame) => { comboBoxMember.Image = frame; });

            else if (ribbonItem is TextBox textBox)
                textBox.Image = image?.GetBitmapFrame(16, (frame) => { textBox.Image = frame; });

            return ribbonItem;
        }
        /// <summary>
        /// Sets the large (32 px) image of <paramref name="ribbonItem"/>, using the image variant for the current Revit theme.
        /// For <see cref="RibbonButton"/> the small image is also updated from the large one when needed;
        /// for other item types only the small image is set.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="largeImage">The image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource largeImage) where TRibbonItem : RibbonItem
        {
            largeImage = largeImage.GetThemeImageSource(RibbonThemeUtils.IsLight);

            if (ribbonItem is RibbonButton ribbonButton)
            {
                ribbonButton.LargeImage = largeImage?.GetBitmapFrame(32, (frame) => { ribbonButton.LargeImage = frame; });
                if (ribbonButton.Image == null || ribbonButton.LargeImage == null || ribbonButton.LargeImage is System.Windows.Media.Imaging.BitmapFrame)
                    ribbonButton.SetImage(ribbonButton.LargeImage);
            }

            else if (ribbonItem is ComboBox comboBox)
                comboBox.SetImage(largeImage);

            else if (ribbonItem is ComboBoxMember comboBoxMember)
                comboBoxMember.SetImage(largeImage);

            else if (ribbonItem is TextBox textBox)
                textBox.SetImage(largeImage);

            return ribbonItem;
        }

        /// <summary>
        /// Sets the size of the images in the drop-down list of <paramref name="pulldownButton"/>.
        /// </summary>
        /// <typeparam name="TPulldownButton">Type of the pulldown button.</typeparam>
        /// <param name="pulldownButton">The pulldown or split button.</param>
        /// <param name="listImageSize">The image size used in the drop-down list.</param>
        /// <returns>The same <paramref name="pulldownButton"/>.</returns>
        public static TPulldownButton SetListImageSize<TPulldownButton>(
            this TPulldownButton pulldownButton,
            Autodesk.Windows.RibbonImageSize listImageSize = Autodesk.Windows.RibbonImageSize.Standard) where TPulldownButton : PulldownButton
        {
            if (pulldownButton.ToAWRibbonItem() is Autodesk.Windows.RibbonSplitButton ribbonSplitButton)
                ribbonSplitButton.ListImageSize = listImageSize;

            return pulldownButton;
        }
    }
}