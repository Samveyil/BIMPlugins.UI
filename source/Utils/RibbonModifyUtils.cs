using Autodesk.Revit.UI;
using System.Linq;

namespace BIMPlugins.UI.Utils
{
    public static class RibbonModifyUtils
    {
        private const string MODIFY_PANEL_ID = "Modify";

        public static Autodesk.Windows.RibbonTab RibbonTab()
        {
            return RibbonTabExtension.GetRibbonTab(MODIFY_PANEL_ID);
        }

        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName, params RibbonItem[] ribbonItems)
        {
            var panel = CreateRibbonPanel(panelName, ribbonItems.Select(e => e.ToAWRibbonItem()).ToArray());
            return panel;
        }
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName, params Autodesk.Windows.RibbonItem[] ribbonItems)
        {
            var panel = CreateRibbonPanel(panelName);
            foreach (var item in ribbonItems)
            {
                panel.Source.Items.Add(item);
            }
            return panel;
        }
        public static Autodesk.Windows.RibbonPanel CreateRibbonPanel(string panelName)
        {
            var modifyTab = RibbonTab();
            var modifyPanel = modifyTab.CreateRibbonPanel(panelName);
            return modifyPanel;
        }

        public static Autodesk.Windows.RibbonPanel RemoveRibbonPanel(string panelName)
        {
            var modifyTab = RibbonTab();
            var modifyPanel = modifyTab.RemoveRibbonPanel(panelName);
            return modifyPanel;
        }
    }
}