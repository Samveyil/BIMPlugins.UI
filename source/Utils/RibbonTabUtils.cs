namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Helpers to add and remove AdWindows panels in a <see cref="Autodesk.Windows.RibbonTab"/>.
    /// </summary>
    internal static class RibbonTabUtils
    {
        /// <summary>
        /// Gets the panel id used for <paramref name="panelName"/>.
        /// </summary>
        /// <param name="panelName">The panel title.</param>
        /// <returns>The panel id in the format <c>ID_{panelName}</c>.</returns>
        public static string GetPanelId(string panelName) => $"ID_{panelName}";

        /// <summary>
        /// Creates an empty panel titled <paramref name="panelName"/> and adds it to <paramref name="tab"/>.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="panelName">The panel title; the panel id is <c>ID_{panelName}</c>.</param>
        /// <returns>The created <see cref="Autodesk.Windows.RibbonPanel"/>.</returns>
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(this Autodesk.Windows.RibbonTab tab, string panelName)
        {
            var panel = new Autodesk.Windows.RibbonPanel
            {
                Source = new Autodesk.Windows.RibbonPanelSource
                {
                    Id = GetPanelId(panelName),
                    Title = panelName
                },
                FloatingOrientation = System.Windows.Controls.Orientation.Vertical
            };

            tab.Panels.Add(panel);
            return panel;
        }

        /// <summary>
        /// Removes the panel created with <see cref="CreateRibbonPanel"/> from <paramref name="tab"/>.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="panelName">The panel title used when the panel was created.</param>
        /// <returns>The removed <see cref="Autodesk.Windows.RibbonPanel"/>, or <c>null</c> if it was not found.</returns>
        public static Autodesk.Windows.RibbonPanel RemoveRibbonPanel(this Autodesk.Windows.RibbonTab tab, string panelName)
        {
            var panel = tab.FindPanel(GetPanelId(panelName));
            if (panel is not null)
            {
                tab.Panels.Remove(panel);
            }
            return panel;
        }
    }
}