namespace BIMPlugins.UI.Utils
{
    internal static class RibbonTabUtils
    {
        public static string GetPanelId(string panelName) => $"ID_{panelName}";

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