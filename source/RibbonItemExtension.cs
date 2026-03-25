using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;
using System.Windows.Media;

namespace BIMPlugins.UI
{
    public static class RibbonItemExtension
    {
        public static TRibbonItem SetContextualHelp<TRibbonItem>(this TRibbonItem ribbonItem, string helpPath) where TRibbonItem : RibbonItem
        {
            if (helpPath != null)
                ribbonItem.SetContextualHelp(RibbonHelpExtension.GetContextualHelp(helpPath));

            return ribbonItem;
        }

        public static TRibbonItem SetShowText<TRibbonItem>(this TRibbonItem ribbonItem, bool showText = false) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().ShowText = showText;
            return ribbonItem;
        }

        public static TRibbonItem SetShowImage<TRibbonItem>(this TRibbonItem ribbonItem, bool showImage = false) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().ShowImage = showImage;
            return ribbonItem;
        }

        public static TRibbonItem SetItemSize<TRibbonItem>(this TRibbonItem ribbonItem, Autodesk.Windows.RibbonItemSize itemSize = Autodesk.Windows.RibbonItemSize.Large) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().Size = itemSize;
            return ribbonItem;
        }

        public static TRibbonItem SetText<TRibbonItem>(this TRibbonItem ribbonItem, string text = "") where TRibbonItem : RibbonItem
        {
            if (text == null)
                return ribbonItem;

            if (text.Trim() == string.Empty)
                return ribbonItem.SetShowText(false);

            ribbonItem.ItemText = text;

            return ribbonItem;
        }

        public static TRibbonItem SetToolTip<TRibbonItem>(this TRibbonItem ribbonItem, string toolTip) where TRibbonItem : RibbonItem
        {
            if (toolTip != null)
                ribbonItem.ToolTip = toolTip;

            return ribbonItem;
        }

        public static TRibbonItem SetLongDescription<TRibbonItem>(this TRibbonItem ribbonItem, string longDescription) where TRibbonItem : RibbonItem
        {
            if (longDescription != null)
                ribbonItem.LongDescription = longDescription;

            return ribbonItem;
        }

        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource toolTipImage) where TRibbonItem : RibbonItem
        {
            ribbonItem.ToolTipImage = toolTipImage;
            return ribbonItem;
        }
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string toolTipImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = toolTipImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(toolTipImage))
                return ribbonItem;

            return ribbonItem.SetToolTipImage(bitmapSource);
        }

        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string image) where TRibbonItem : RibbonItem
        {
            var bitmapSource = image?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(image))
                return ribbonItem;

            return ribbonItem.SetImage(bitmapSource);
        }
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string largeImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = largeImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(largeImage))
                return ribbonItem;

            return ribbonItem.SetLargeImage(bitmapSource);
        }

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