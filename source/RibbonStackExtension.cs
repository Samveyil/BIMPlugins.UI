using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to stack <see cref="RibbonItem"/> already added to a <see cref="RibbonPanel"/>.
    /// </summary>
    public static class RibbonStackExtension
    {
        /// <summary>
        /// Moves <paramref name="ribbonItems"/> into stacked rows, with up to 3 items per stack.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to stack.</param>
        /// <returns>The created row panels, one for every group of up to 3 items.</returns>
        public static Autodesk.Windows.RibbonRowPanel[] RowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(ribbonItems);
        }

        /// <summary>
        /// Moves <paramref name="ribbonItems"/> into stacked rows of large items, with up to 2 items per stack.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to stack.</param>
        /// <returns>The created row panels, one for every group of up to 2 items.</returns>
        public static Autodesk.Windows.RibbonRowPanel[] RowLargeStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(2, ribbonItems).SetRibbonItemSize();
        }

        /// <summary>
        /// Moves <paramref name="ribbonItems"/> into a flow panel that shows them as small image-only items.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to place in the flow panel.</param>
        /// <returns>The created flow panel, or <c>null</c> if <paramref name="ribbonItems"/> is empty.</returns>
        public static Autodesk.Windows.RibbonFlowPanel FlowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateFlowStackedItems(ribbonItems);
        }
    }
}