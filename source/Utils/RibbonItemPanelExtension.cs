using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Helpers to group ribbon items into stacked rows (<see cref="Autodesk.Windows.RibbonRowPanel"/>) and flow panels (<see cref="Autodesk.Windows.RibbonFlowPanel"/>).
    /// </summary>
    public static class RibbonItemPanelExtension
    {
        private const int MaxRowNumber = 3;
        /// <summary>
        /// Moves <paramref name="ribbonItems"/> into stacked rows, with up to 3 items per stack.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to stack.</param>
        /// <returns>The created row panels.</returns>
        internal static Autodesk.Windows.RibbonRowPanel[] CreateRowStackedItemsWithMax(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(MaxRowNumber, ribbonItems);
        }
        /// <summary>
        /// Moves <paramref name="ribbonItems"/> into stacked rows, with up to <paramref name="maxRowNumber"/> items per stack.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="maxRowNumber">The maximum number of items in one stack.</param>
        /// <param name="ribbonItems">The items to stack.</param>
        /// <returns>The created row panels, or an empty array if <paramref name="ribbonItems"/> is empty.</returns>
        internal static Autodesk.Windows.RibbonRowPanel[] CreateRowStackedItemsWithMax(this RibbonPanel ribbonPanel, int maxRowNumber, params RibbonItem[] ribbonItems)
        {
            var ribbonFlowPanels = new List<Autodesk.Windows.RibbonRowPanel>();

            if (ribbonItems.Length == 0)
                return ribbonFlowPanels.ToArray();

            var list = new List<RibbonItem>();
            for (int i = 0; i < ribbonItems.Length; i++)
            {
                var ribbonItem = ribbonItems[i];
                list.Add(ribbonItem);
                if (list.Count == maxRowNumber)
                {
                    var ribbonFlowPanel = ribbonPanel.CreateRowStackedItems(list.ToArray());
                    ribbonFlowPanels.Add(ribbonFlowPanel);
                    list.Clear();
                }
            }

            if (list.Any())
                ribbonFlowPanels.Add(ribbonPanel.CreateRowStackedItems(list.ToArray()));

            return ribbonFlowPanels.ToArray();
        }
        /// <summary>
        /// Removes <paramref name="ribbonItems"/> from <paramref name="ribbonPanel"/> and adds them back as one stack, one item per row.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to stack.</param>
        /// <returns>The created row panel, or <c>null</c> if <paramref name="ribbonItems"/> is empty.</returns>
        internal static Autodesk.Windows.RibbonRowPanel CreateRowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            if (ribbonItems.Length == 0) return null;

            var ribbonFlowPanel = new Autodesk.Windows.RibbonRowPanel();
            var ribbonItemLast = ribbonItems.LastOrDefault();
            foreach (var ribbonItem in ribbonItems)
            {
                var awRibbonItem = ribbonItem.ToAWRibbonItem();
                ribbonPanel.Remove(ribbonItem);
                ribbonFlowPanel.AddRibbonItem(awRibbonItem);

                if (ribbonItemLast != ribbonItem)
                    ribbonFlowPanel.Items.Add(new Autodesk.Windows.RibbonRowBreak());
            }
            ribbonPanel.ToAWRibbonPanel().Source.Items.Add(ribbonFlowPanel);
            return ribbonFlowPanel;
        }

        /// <summary>
        /// Adds <paramref name="ribbonItem"/> to <paramref name="ribbonRowPanel"/> as a standard size item with horizontal text.
        /// </summary>
        /// <param name="ribbonRowPanel">The row panel.</param>
        /// <param name="ribbonItem">The item to add.</param>
        /// <returns>The same <paramref name="ribbonRowPanel"/>.</returns>
        public static Autodesk.Windows.RibbonRowPanel AddRibbonItem(this Autodesk.Windows.RibbonRowPanel ribbonRowPanel, Autodesk.Windows.RibbonItem ribbonItem)
        {
            ribbonRowPanel.Items.Add(UpdateForRibbonRowPanel(ribbonItem));
            return ribbonRowPanel;
        }

        /// <summary>
        /// Sets the size of all items in <paramref name="ribbonRowPanels"/>.
        /// </summary>
        /// <param name="ribbonRowPanels">The row panels.</param>
        /// <param name="ribbonItemSize">The item size.</param>
        /// <returns>The same <paramref name="ribbonRowPanels"/>.</returns>
        public static Autodesk.Windows.RibbonRowPanel[] SetRibbonItemSize(
            this Autodesk.Windows.RibbonRowPanel[] ribbonRowPanels,
            Autodesk.Windows.RibbonItemSize ribbonItemSize = Autodesk.Windows.RibbonItemSize.Large)
        {
            foreach (var ribbonRowPanel in ribbonRowPanels)
            {
                ribbonRowPanel.SetRibbonItemSize(ribbonItemSize);
            }
            return ribbonRowPanels;
        }
        /// <summary>
        /// Sets the size of all items in <paramref name="ribbonRowPanel"/>.
        /// </summary>
        /// <param name="ribbonRowPanel">The row panel.</param>
        /// <param name="ribbonItemSize">The item size.</param>
        /// <returns>The same <paramref name="ribbonRowPanel"/>.</returns>
        public static Autodesk.Windows.RibbonRowPanel SetRibbonItemSize(
            this Autodesk.Windows.RibbonRowPanel ribbonRowPanel,
            Autodesk.Windows.RibbonItemSize ribbonItemSize = Autodesk.Windows.RibbonItemSize.Large)
        {
            foreach (var ribbonItem in ribbonRowPanel.Items)
            {
                ribbonItem.Size = ribbonItemSize;
            }
            return ribbonRowPanel;
        }

        /// <summary>
        /// Prepares <paramref name="ribbonItem"/> for a row panel: standard size, hidden in the status bar and horizontal text.
        /// </summary>
        /// <param name="ribbonItem">The item.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        internal static Autodesk.Windows.RibbonItem UpdateForRibbonRowPanel(this Autodesk.Windows.RibbonItem ribbonItem)
        {
            ribbonItem.Size = Autodesk.Windows.RibbonItemSize.Standard;
            ribbonItem.AllowInStatusBar = false;
            ribbonItem.SetTextOrientation(System.Windows.Controls.Orientation.Horizontal);
            return ribbonItem;
        }
        /// <summary>
        /// Sets the text orientation of <paramref name="ribbonItem"/> if the item supports it.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="ribbonItem">The item.</param>
        /// <param name="orientation">The text orientation.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        internal static T SetTextOrientation<T>(this T ribbonItem, System.Windows.Controls.Orientation orientation) where T : Autodesk.Windows.RibbonItem
        {
            try
            {
                var propriety = ribbonItem.GetType().GetProperty(nameof(Autodesk.Private.Windows.IRibbonTextProperties.Orientation));
                propriety?.SetValue(ribbonItem, orientation);
            }
            catch { }
            return ribbonItem;
        }

        /// <summary>
        /// Removes <paramref name="ribbonItems"/> from <paramref name="ribbonPanel"/> and adds them back into one flow panel.
        /// </summary>
        /// <param name="ribbonPanel">The panel that contains the items.</param>
        /// <param name="ribbonItems">The items to place in the flow panel.</param>
        /// <returns>The created flow panel, or <c>null</c> if <paramref name="ribbonItems"/> is empty.</returns>
        internal static Autodesk.Windows.RibbonFlowPanel CreateFlowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            if (ribbonItems.Length == 0) return null;

            var ribbonFlowPanel = new Autodesk.Windows.RibbonFlowPanel();
            foreach (var ribbonItem in ribbonItems)
            {
                var awRibbonItem = ribbonItem.ToAWRibbonItem();
                ribbonPanel.Remove(ribbonItem);
                ribbonFlowPanel.AddRibbonItem(awRibbonItem);
            }

            ribbonPanel.ToAWRibbonPanel().Source.Items.Add(ribbonFlowPanel);
            return ribbonFlowPanel;
        }

        /// <summary>
        /// Adds <paramref name="ribbonItem"/> to <paramref name="ribbonFlowPanel"/> as a standard size, image-only item.
        /// </summary>
        /// <param name="ribbonFlowPanel">The flow panel.</param>
        /// <param name="ribbonItem">The item to add.</param>
        /// <returns>The same <paramref name="ribbonFlowPanel"/>.</returns>
        public static Autodesk.Windows.RibbonFlowPanel AddRibbonItem(this Autodesk.Windows.RibbonFlowPanel ribbonFlowPanel, Autodesk.Windows.RibbonItem ribbonItem)
        {
            ribbonFlowPanel.Items.Add(UpdateForRibbonFlowPanel(ribbonItem));
            return ribbonFlowPanel;
        }
        /// <summary>
        /// Prepares <paramref name="ribbonItem"/> for a flow panel: standard size, image only and hidden in the status bar.
        /// </summary>
        /// <param name="ribbonItem">The item.</param>
        /// <returns>The same <paramref name="ribbonItem"/>.</returns>
        internal static Autodesk.Windows.RibbonItem UpdateForRibbonFlowPanel(this Autodesk.Windows.RibbonItem ribbonItem)
        {
            ribbonItem.Size = Autodesk.Windows.RibbonItemSize.Standard;
            ribbonItem.ShowImage = true;
            ribbonItem.ShowText = false;
            ribbonItem.AllowInStatusBar = false;
            return ribbonItem;
        }
    }
}