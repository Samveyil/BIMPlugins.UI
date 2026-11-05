using Autodesk.Revit.UI;
using System.Linq;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Helpers to add and remove panels in the Revit Modify tab.
    /// </summary>
    public static class RibbonModifyUtils
    {
        private const string MODIFY_PANEL_ID = "Modify";

        /// <summary>
        /// Gets the Revit Modify tab.
        /// </summary>
        /// <returns>The Modify <see cref="Autodesk.Windows.RibbonTab"/>.</returns>
        public static Autodesk.Windows.RibbonTab RibbonTab()
        {
            return RibbonTabExtension.GetRibbonTab(MODIFY_PANEL_ID);
        }

        /// <summary>
        /// Creates a panel in the Modify tab containing <paramref name="ribbonItems"/>.
        /// </summary>
        /// <param name="panelName">The panel title; the panel id is <c>ID_{panelName}</c>.</param>
        /// <param name="ribbonItems">The Revit API items to add to the panel.</param>
        /// <returns>The created <see cref="Autodesk.Windows.RibbonPanel"/>.</returns>
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName, params RibbonItem[] ribbonItems)
        {
            var panel = CreateRibbonPanel(panelName, ribbonItems.Select(e => e.ToAWRibbonItem()).ToArray());
            return panel;
        }
        /// <summary>
        /// Creates a panel in the Modify tab containing <paramref name="ribbonItems"/>.
        /// </summary>
        /// <param name="panelName">The panel title; the panel id is <c>ID_{panelName}</c>.</param>
        /// <param name="ribbonItems">The AdWindows items to add to the panel.</param>
        /// <returns>The created <see cref="Autodesk.Windows.RibbonPanel"/>.</returns>
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName, params Autodesk.Windows.RibbonItem[] ribbonItems)
        {
            var panel = CreateRibbonPanel(panelName);
            foreach (var item in ribbonItems)
            {
                panel.Source.Items.Add(item);
            }
            return panel;
        }
        /// <summary>
        /// Creates an empty panel in the Modify tab.
        /// </summary>
        /// <param name="panelName">The panel title; the panel id is <c>ID_{panelName}</c>.</param>
        /// <returns>The created <see cref="Autodesk.Windows.RibbonPanel"/>.</returns>
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName)
        {
            var modifyTab = RibbonTab();
            var modifyPanel = modifyTab.CreateRibbonPanel(panelName);
            return modifyPanel;
        }

        /// <summary>
        /// Removes the panel created with <see cref="CreateRibbonPanel(string)"/> from the Modify tab.
        /// </summary>
        /// <param name="panelName">The panel title used when the panel was created.</param>
        /// <returns>The removed <see cref="Autodesk.Windows.RibbonPanel"/>, or <c>null</c> if it was not found.</returns>
        public static Autodesk.Windows.RibbonPanel RemoveRibbonPanel(string panelName)
        {
            var modifyTab = RibbonTab();
            var modifyPanel = modifyTab.RemoveRibbonPanel(panelName);
            return modifyPanel;
        }
    }
}