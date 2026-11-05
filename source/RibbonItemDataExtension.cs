using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;
using System.Windows.Media;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Fluent extension methods to configure <see cref="RibbonItemData"/> before it is added to a panel.
    /// </summary>
    public static class RibbonItemDataExtension
    {
        /// <summary>
        /// Sets the contextual help (F1) of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="helpPath">An url (starting with <c>http</c>) or a path to a CHM file. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetContextualHelp<TRibbonItem>(this TRibbonItem ribbonItem, string helpPath) where TRibbonItem : RibbonItemData
        {
            if (helpPath != null)
                ribbonItem.SetContextualHelp(RibbonHelpExtension.GetContextualHelp(helpPath));

            return ribbonItem;
        }

        /// <summary>
        /// Sets the tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="toolTip">The tooltip text. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTip<TRibbonItem>(this TRibbonItem ribbonItem, string toolTip) where TRibbonItem : RibbonItemData
        {
            if (toolTip != null)
                ribbonItem.ToolTip = toolTip;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the long description shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="longDescription">The long description. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLongDescription<TRibbonItem>(this TRibbonItem ribbonItem, string longDescription) where TRibbonItem : RibbonItemData
        {
            if (longDescription != null)
                ribbonItem.LongDescription = longDescription;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the image shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="toolTipImage">The tooltip image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource toolTipImage) where TRibbonItem : RibbonItemData
        {
            ribbonItem.ToolTipImage = toolTipImage;
            return ribbonItem;
        }

        /// <summary>
        /// Sets the image shown in the extended tooltip of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="toolTipImage">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string toolTipImage) where TRibbonItem : RibbonItemData
        {
            var bitmapSource = toolTipImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(toolTipImage))
                return ribbonItem;

            return ribbonItem.SetToolTipImage(bitmapSource);
        }

        /// <summary>
        /// Sets the text of <paramref name="ribbonItem"/> if it is a <see cref="ButtonData"/> or <see cref="ComboBoxMemberData"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="text">The text. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetText<TRibbonItem>(this TRibbonItem ribbonItem, string text = "") where TRibbonItem : RibbonItemData
        {
            if (text == null)
                return ribbonItem;

            if (ribbonItem is ButtonData button)
                button.Text = text;

            if (ribbonItem is ComboBoxMemberData comboBoxMemberData)
                comboBoxMemberData.Text = text;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the small (16 px) image of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="image">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string image) where TRibbonItem : RibbonItemData
        {
            var bitmapSource = image?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(image))
                return ribbonItem;

            return ribbonItem.SetImage(bitmapSource);
        }
        /// <summary>
        /// Sets the large (32 px) image of <paramref name="ribbonItem"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="largeImage">Base64 string or uri of the image, see <see cref="BitmapExtension.GetBitmapSource"/>. If the image cannot be loaded, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string largeImage) where TRibbonItem : RibbonItemData
        {
            var bitmapSource = largeImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(largeImage))
                return ribbonItem;

            return ribbonItem.SetLargeImage(bitmapSource);
        }

        /// <summary>
        /// Sets the small (16 px) image of <paramref name="ribbonItem"/>, using the image variant for the current Revit theme.
        /// Supports <see cref="ButtonData"/>, <see cref="ComboBoxData"/>, <see cref="ComboBoxMemberData"/> and <see cref="TextBoxData"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="image">The image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource image) where TRibbonItem : RibbonItemData
        {
            image = image.GetThemeImageSource(RibbonThemeUtils.IsLight);

            if (ribbonItem is ButtonData ribbonButton)
                ribbonButton.Image = image?.GetBitmapFrame(16, (frame) => { ribbonButton.Image = frame; });

            else if (ribbonItem is ComboBoxData comboBox)
                comboBox.Image = image?.GetBitmapFrame(16, (frame) => { comboBox.Image = frame; });

            else if (ribbonItem is ComboBoxMemberData comboBoxMemberData)
                comboBoxMemberData.Image = image?.GetBitmapFrame(16, (frame) => { comboBoxMemberData.Image = frame; });

            else if (ribbonItem is TextBoxData textBoxData)
                textBoxData.Image = image?.GetBitmapFrame(16, (frame) => { textBoxData.Image = frame; });

            return ribbonItem;
        }
        /// <summary>
        /// Sets the large (32 px) image of <paramref name="ribbonItem"/>, using the image variant for the current Revit theme.
        /// For <see cref="ButtonData"/> the small image is also updated from the large one when needed;
        /// for other item types only the small image is set.
        /// </summary>
        /// <typeparam name="TRibbonItem">Type of the ribbon item data.</typeparam>
        /// <param name="ribbonItem">The ribbon item data.</param>
        /// <param name="largeImage">The image.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource largeImage) where TRibbonItem : RibbonItemData
        {
            largeImage = largeImage.GetThemeImageSource(RibbonThemeUtils.IsLight);

            if (ribbonItem is ButtonData ribbonButton)
            {
                ribbonButton.LargeImage = largeImage?.GetBitmapFrame(32, (frame) => { ribbonButton.LargeImage = frame; });
                if (ribbonButton.Image == null || ribbonButton.LargeImage == null || ribbonButton.LargeImage is System.Windows.Media.Imaging.BitmapFrame)
                    ribbonButton.SetImage(ribbonButton.LargeImage);
            }

            else if (ribbonItem is ComboBoxData comboBox)
                comboBox.SetImage(largeImage);

            else if (ribbonItem is ComboBoxMemberData comboBoxMemberData)
                comboBoxMemberData.SetImage(largeImage);

            else if (ribbonItem is TextBoxData textBoxData)
                textBoxData.SetImage(largeImage);

            return ribbonItem;
        }
    }
}