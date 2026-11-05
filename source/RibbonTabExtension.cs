using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to find, create, remove and order ribbon tabs.
    /// </summary>
    public static class RibbonTabExtension
    {
        /// <summary>
        /// Creates the ribbon tab <paramref name="tabName"/> if no tab with this name exists.
        /// </summary>
        /// <param name="application">The Revit application.</param>
        /// <param name="tabName">The tab name.</param>
        public static void FindOrCreateRibbonTab(this UIControlledApplication application, string tabName)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;

            if (!ribbon.Tabs.Any(tab => tab.AutomationName == tabName))
            {
                application.CreateRibbonTab(tabName);
            }
        }

        /// <summary>
        /// Gets the tab that contains <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <returns>The <see cref="Autodesk.Windows.RibbonTab"/> of the panel.</returns>
        public static Autodesk.Windows.RibbonTab GetRibbonTab(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.ToAWRibbonPanel().Tab;
        }
        /// <summary>
        /// Gets the ribbon tab with id <paramref name="ribbonTabId"/>.
        /// </summary>
        /// <param name="ribbonTabId">The tab id.</param>
        /// <returns>The found <see cref="Autodesk.Windows.RibbonTab"/>, or <c>null</c>.</returns>
        public static Autodesk.Windows.RibbonTab GetRibbonTab(string ribbonTabId)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            return ribbon.FindTab(ribbonTabId);
        }

        /// <summary>
        /// Gets all tabs of the Revit ribbon.
        /// </summary>
        /// <returns>The list of ribbon tabs.</returns>
        public static IList<Autodesk.Windows.RibbonTab> GetRibbonTabs()
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            return ribbon.Tabs;
        }

        /// <summary>
        /// Removes <paramref name="ribbonTab"/> from the Revit ribbon and from the internal Revit tab dictionary.
        /// </summary>
        /// <param name="ribbonTab">The tab to remove.</param>
        /// <returns><c>true</c> if the tab was removed.</returns>
        public static bool Remove(this Autodesk.Windows.RibbonTab ribbonTab)
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            GetRibbonTabsDictionary()?.Remove(ribbonTab.Id);
            return ribbon.Tabs.Remove(ribbonTab);
        }
        /// <summary>
        /// Removes <paramref name="ribbonPanel"/> from <paramref name="ribbonTab"/> and from the internal Revit panel dictionary of the tab.
        /// </summary>
        /// <param name="ribbonTab">The tab.</param>
        /// <param name="ribbonPanel">The panel to remove.</param>
        /// <returns><c>true</c> if the panel was removed.</returns>
        public static bool Remove(this Autodesk.Windows.RibbonTab ribbonTab, Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            var removed = ribbonTab.Panels.Remove(ribbonPanel);
            GetRibbonTabsDictionary(ribbonTab)?.Remove(ribbonPanel.Source.Name);
            return removed;
        }

        /// <summary>
        /// Moves <paramref name="ribbonPanel"/> from its current tab to <paramref name="ribbonTab"/>.
        /// </summary>
        /// <param name="ribbonTab">The target tab.</param>
        /// <param name="ribbonPanel">The panel to move.</param>
        /// <returns><c>true</c> if the panel was removed from its previous tab.</returns>
        internal static bool MoveToRibbonTab(this Autodesk.Windows.RibbonTab ribbonTab, Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            var removed = ribbonPanel.Tab.Remove(ribbonPanel);
            ribbonTab.Panels.Add(ribbonPanel);
            return removed;
        }

        /// <summary>
        /// Orders the panels of <paramref name="ribbonTab"/> by <paramref name="keySelector"/>.
        /// </summary>
        /// <typeparam name="TKey">Type of the key used to order the panels.</typeparam>
        /// <param name="ribbonTab">The tab.</param>
        /// <param name="keySelector">Function that returns the key of a panel.</param>
        /// <returns>The same <paramref name="ribbonTab"/>.</returns>
        public static Autodesk.Windows.RibbonTab SetPanelsOrderBy<TKey>(this Autodesk.Windows.RibbonTab ribbonTab, Func<Autodesk.Windows.RibbonPanel, TKey> keySelector)
        {
            if (ribbonTab.Panels.Count <= 1)
                return ribbonTab;

            ribbonTab.Panels.OrderBy(keySelector);
            return ribbonTab;
        }
        /// <summary>
        /// Orders the panels of <paramref name="ribbonTab"/> by title.
        /// </summary>
        /// <param name="ribbonTab">The tab.</param>
        /// <returns>The same <paramref name="ribbonTab"/>.</returns>
        public static Autodesk.Windows.RibbonTab SetPanelsOrderByTitle(this Autodesk.Windows.RibbonTab ribbonTab)
        {
            return ribbonTab.SetPanelsOrderBy(ComparationOrderByTitle);
        }

        /// <summary>
        /// Gets the title of <paramref name="ribbonPanel"/>, used to order panels by title.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <returns>The panel title.</returns>
        private static string ComparationOrderByTitle(Autodesk.Windows.RibbonPanel ribbonPanel)
        {
            return ribbonPanel.Source.Title;
        }

        /// <summary>
        /// Gets the internal Revit dictionary of panels for <paramref name="ribbonTab"/>.
        /// </summary>
        /// <param name="ribbonTab">The tab.</param>
        /// <returns>The panels of the tab by name, or <c>null</c> if the tab was not created through the Revit API.</returns>
        internal static Dictionary<string, RibbonPanel> GetRibbonTabsDictionary(Autodesk.Windows.RibbonTab ribbonTab)
        {
            return GetRibbonTabsDictionary(ribbonTab.Id);
        }
        /// <summary>
        /// Gets the internal Revit dictionary of panels for the tab with id <paramref name="ribbonTabId"/>.
        /// </summary>
        /// <param name="ribbonTabId">The tab id.</param>
        /// <returns>The panels of the tab by name, or <c>null</c> if the tab was not created through the Revit API.</returns>
        internal static Dictionary<string, RibbonPanel> GetRibbonTabsDictionary(string ribbonTabId)
        {
            if (GetRibbonTabsDictionary().TryGetValue(ribbonTabId, out Dictionary<string, RibbonPanel> value))
                return value;

            return null;
        }
        /// <summary>
        /// Gets the internal Revit dictionary <c>UIApplication.RibbonItemDictionary</c> through reflection.
        /// </summary>
        /// <returns>The panels by tab id and panel name, or <c>null</c> if it could not be read.</returns>
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