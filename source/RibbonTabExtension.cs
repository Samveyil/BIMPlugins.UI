using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonTabExtension
    {
        public static void FindOrCreateRibbonTab(this UIControlledApplication application, string tabName)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;

            if (!ribbon.Tabs.Any(tab => tab.AutomationName == tabName))
            {
                application.CreateRibbonTab(tabName);
            }
        }

        public static Autodesk.Windows.RibbonTab GetRibbonTab(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.ToAWRibbonPanel().Tab;
        }
        public static Autodesk.Windows.RibbonTab GetRibbonTab(string ribbonTabId)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            return ribbon.FindTab(ribbonTabId);
        }

        public static IList<Autodesk.Windows.RibbonTab> GetRibbonTabs()
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            return ribbon.Tabs;
        }

        public static bool Remove(this Autodesk.Windows.RibbonTab ribbonTab)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            GetRibbonTabsDictionary()?.Remove(ribbonTab.Id);
            return ribbon.Tabs.Remove(ribbonTab);
        }
        public static bool Remove(this Autodesk.Windows.RibbonTab ribbonTab, Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            var removed = ribbonTab.Panels.Remove(ribbonPanel);
            GetRibbonTabsDictionary(ribbonTab)?.Remove(ribbonPanel.Source.Name);
            return removed;
        }

        internal static bool MoveToRibbonTab(this Autodesk.Windows.RibbonTab ribbonTab, Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            var removed = ribbonPanel.Tab.Remove(ribbonPanel);
            ribbonTab.Panels.Add(ribbonPanel);
            return removed;
        }
        
        public static Autodesk.Windows.RibbonTab SetPanelsOrderBy<TKey>(this Autodesk.Windows.RibbonTab ribbonTab, Func<Autodesk.Windows.RibbonPanel, TKey> keySelector)
        {
            if (ribbonTab.Panels.Count <= 1)
                return ribbonTab;

            ribbonTab.Panels.OrderBy(keySelector);
            return ribbonTab;
        }
        public static Autodesk.Windows.RibbonTab SetPanelsOrderByTitle(this Autodesk.Windows.RibbonTab ribbonTab)
        {
            return ribbonTab.SetPanelsOrderBy(ComparationOrderByTitle);
        }

        private static string ComparationOrderByTitle(Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            return ribbonPanel.Source.Title;
        }

        internal static Dictionary<string, RibbonPanel> GetRibbonTabsDictionary(Autodesk.Windows.RibbonTab ribbonTab)
        {
            return GetRibbonTabsDictionary(ribbonTab.Id);
        }
        internal static Dictionary<string, RibbonPanel> GetRibbonTabsDictionary(string ribbonTabId)
        {
            if (GetRibbonTabsDictionary().TryGetValue(ribbonTabId, out Dictionary<string, RibbonPanel> value))
                return value;

            return null;
        }
        internal static Dictionary<string, Dictionary<string, RibbonPanel>> GetRibbonTabsDictionary()
        {
            var type = typeof(UIApplication);

            var ribbonItemDictionary = type.GetProperty("RibbonItemDictionary",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.GetValue(null);

            return ribbonItemDictionary as Dictionary<string, Dictionary<string, RibbonPanel>>;
        }
    }
}