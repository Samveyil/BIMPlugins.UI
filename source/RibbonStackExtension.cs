using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;

namespace BIMPlugins.UI
{
    public static class RibbonStackExtension
    {
        public static Autodesk.Windows.RibbonRowPanel[] RowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(ribbonItems);
        }

        public static Autodesk.Windows.RibbonRowPanel[] RowLargeStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(2, ribbonItems).SetRibbonItemSize();
        }

        public static Autodesk.Windows.RibbonFlowPanel FlowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateFlowStackedItems(ribbonItems);
        }
    }
}