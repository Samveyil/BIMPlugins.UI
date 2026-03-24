using Autodesk.Revit.UI;
using System.Windows.Media;

namespace BIMPlugins.UI
{
    public static class RibbonItemExtension
    {
        public static TRibbonItem SetShowText<TRibbonItem>(this TRibbonItem ribbonItem, bool showText = false) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToAWRibbonItem() != null)
                ribbonItem.ToAWRibbonItem().ShowText = showText;
            return ribbonItem;
        }

        public static TRibbonItem SetContextualHelp<TRibbonItem>(this TRibbonItem ribbonItem, string helpPath) where TRibbonItem : RibbonItem
        {
            if (helpPath != null)
                ribbonItem.SetContextualHelp(RibbonHelp.GetContextualHelp(helpPath));

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

        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string toolTipImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = toolTipImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(toolTipImage))
                return ribbonItem;

            return ribbonItem.SetToolTipImage(bitmapSource);
        }
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string assemblyName, string toolTipImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = toolTipImage?.GetBitmapSource(assemblyName);

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(toolTipImage))
                return ribbonItem;

            return ribbonItem.SetToolTipImage(bitmapSource);
        }
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource toolTipImage) where TRibbonItem : RibbonItem
        {
            ribbonItem.ToolTipImage = toolTipImage;
            return ribbonItem;
        }

        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string image) where TRibbonItem : RibbonItem
        {
            var bitmapSource = image?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(image))
                return ribbonItem;

            return ribbonItem.SetImage(bitmapSource);
        }
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string assemblyName, string image) where TRibbonItem : RibbonItem
        {
            var bitmapSource = image?.GetBitmapSource(assemblyName);

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(image))
                return ribbonItem;

            return ribbonItem.SetImage(bitmapSource);
        }
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource image) where TRibbonItem : RibbonItem
        {
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

        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string largeImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = largeImage?.GetBitmapSource();

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(largeImage))
                return ribbonItem;

            return ribbonItem.SetLargeImage(bitmapSource);
        }
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string assemblyName, string largeImage) where TRibbonItem : RibbonItem
        {
            var bitmapSource = largeImage?.GetBitmapSource(assemblyName);

            if (bitmapSource == null && !string.IsNullOrWhiteSpace(largeImage))
                return ribbonItem;

            return ribbonItem.SetLargeImage(bitmapSource);
        }
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource largeImage) where TRibbonItem : RibbonItem
        {
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