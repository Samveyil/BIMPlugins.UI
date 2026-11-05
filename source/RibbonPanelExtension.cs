using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create, find, move and remove <see cref="RibbonPanel"/>.
    /// </summary>
    public static class RibbonPanelExtension
    {
        /// <summary>
        /// Creates a <see cref="RibbonPanel"/> in the Add-Ins tab.
        /// If a panel with the same name already exists, a panel with a unique name and the title <paramref name="panelName"/> is created.
        /// </summary>
        /// <param name="application">The Revit application.</param>
        /// <param name="panelName">The panel name and title.</param>
        /// <returns>The created <see cref="RibbonPanel"/>, subscribed to theme changes.</returns>
        public static RibbonPanel CreatePanel(this UIControlledApplication application, string panelName)
        {
            RibbonPanel ribbonPanel;
            try
            {
                ribbonPanel = application.CreateRibbonPanel(panelName);
            }
            catch
            {
                ribbonPanel = application.CreateRibbonPanel(RibbonSafeExtension.SafeRibbonPanelName(panelName));
                ribbonPanel.Title = panelName;
            }
            return ribbonPanel.ThemeChangeEnable();
        }
        /// <summary>
        /// Returns the existing <see cref="RibbonPanel"/> named <paramref name="panelName"/> in the Add-Ins tab, or creates a new one.
        /// </summary>
        /// <param name="application">The Revit application.</param>
        /// <param name="panelName">The panel name.</param>
        /// <returns>The existing or created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreateOrSelectPanel(this UIControlledApplication application, string panelName)
        {
            if (application.GetRibbonPanels().FirstOrDefault(p => p.IsSelect(panelName)) is RibbonPanel ribbonPanel)
                return ribbonPanel;

            return application.CreatePanel(panelName);
        }

        /// <summary>
        /// Creates a <see cref="RibbonPanel"/> in the tab <paramref name="tabName"/>, creating the tab if it does not exist.
        /// If a panel with the same name already exists, a panel with a unique name and the title <paramref name="panelName"/> is created.
        /// </summary>
        /// <param name="application">The Revit application.</param>
        /// <param name="tabName">The tab name. When empty, the panel is created in the Add-Ins tab.</param>
        /// <param name="panelName">The panel name and title.</param>
        /// <returns>The created <see cref="RibbonPanel"/>, subscribed to theme changes.</returns>
        public static RibbonPanel CreatePanel(this UIControlledApplication application, string tabName, string panelName)
        {
            if (string.IsNullOrEmpty(tabName))
                return application.CreatePanel(panelName);

            RibbonPanel ribbonPanel = null;
            try { application.CreateRibbonTab(tabName); } catch { }
            try
            {
                ribbonPanel = application.CreateRibbonPanel(tabName, panelName);
            }
            catch
            {
                if (ribbonPanel == null)
                {
                    ribbonPanel = application.CreateRibbonPanel(tabName, RibbonSafeExtension.SafeRibbonPanelName(panelName));
                    ribbonPanel.Title = panelName;
                }
            }
            return ribbonPanel.ThemeChangeEnable();
        }
        /// <summary>
        /// Returns the existing <see cref="RibbonPanel"/> named <paramref name="panelName"/> in the tab <paramref name="tabName"/>, or creates a new one.
        /// </summary>
        /// <param name="application">The Revit application.</param>
        /// <param name="tabName">The tab name. When empty, the Add-Ins tab is used.</param>
        /// <param name="panelName">The panel name.</param>
        /// <returns>The existing or created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreateOrSelectPanel(this UIControlledApplication application, string tabName, string panelName)
        {
            if (string.IsNullOrEmpty(tabName))
                return application.CreateOrSelectPanel(panelName);

            if (application.GetRibbonPanels(tabName).FirstOrDefault(p => p.IsSelect(panelName)) is RibbonPanel ribbonPanel)
                return ribbonPanel;

            return application.CreatePanel(tabName, panelName);
        }

        /// <summary>
        /// Removes <paramref name="ribbonPanel"/> from its tab: hides and disables it and unsubscribes it from theme changes.
        /// </summary>
        /// <param name="ribbonPanel">The panel to remove.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        public static RibbonPanel Remove(this RibbonPanel ribbonPanel)
        {
            if (ribbonPanel is null)
                return ribbonPanel;

            ribbonPanel.ThemeChangeDisable();
            ribbonPanel.Visible = false;
            ribbonPanel.Enabled = false;

            // Disable Floating
            ribbonPanel.ToAWRibbonPanel().IsFloating = false;

            ribbonPanel.GetRibbonTab().Remove(ribbonPanel.ToAWRibbonPanel());
            return ribbonPanel;
        }

        /// <summary>
        /// Moves <paramref name="ribbonPanel"/> to the Modify tab.
        /// </summary>
        /// <param name="ribbonPanel">The panel to move.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.MoveToRibbonTab("Modify");
        }
        /// <summary>
        /// Moves <paramref name="ribbonPanel"/> to the tab with id <paramref name="ribbonTabId"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to move.</param>
        /// <param name="ribbonTabId">The id of the target tab. If the tab is not found, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel, string ribbonTabId)
        {
            var ribbonTab = Autodesk.Windows.ComponentManager.Ribbon.FindTab(ribbonTabId);
            return ribbonPanel.MoveToRibbonTab(ribbonTab);
        }
        /// <summary>
        /// Moves <paramref name="ribbonPanel"/> to <paramref name="ribbonTab"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to move.</param>
        /// <param name="ribbonTab">The target tab. When <c>null</c>, nothing is changed.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel, Autodesk.Windows.RibbonTab ribbonTab)
        {
            if (ribbonTab is not null)
            {
                var panel = ribbonPanel.ToAWRibbonPanel();
                ribbonTab.MoveToRibbonTab(panel);
            }
            return ribbonPanel;
        }

        /// <summary>
        /// Gets all items of <paramref name="ribbonPanel"/>, including the items inside pulldown, split, radio group and combo box items.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <returns>The list of items, or an empty list when <paramref name="ribbonPanel"/> is <c>null</c>.</returns>
        public static IList<RibbonItem> GetRibbonItems(this RibbonPanel ribbonPanel)
        {
            var ribbonItems = new List<RibbonItem>();

            if (ribbonPanel is null)
                return ribbonItems;

            foreach (var ribbonItem in ribbonPanel.GetItems())
            {
                ribbonItems.Add(ribbonItem);
                if (ribbonItem is PulldownButton pulldownButton)
                    ribbonItems.AddRange(pulldownButton.GetItems());
                if (ribbonItem is SplitButton splitButton)
                    ribbonItems.AddRange(splitButton.GetItems());
                if (ribbonItem is ToggleButton) { }
                if (ribbonItem is RadioButtonGroup radioButtonGroup)
                {
                    ribbonItems.AddRange(radioButtonGroup.GetItems());
                }
                if (ribbonItem is ComboBoxMember) { }
                if (ribbonItem is ComboBox comboBox)
                {
                    ribbonItems.AddRange(comboBox.GetItems());
                }
                if (ribbonItem is TextBox) { }
            }
            return ribbonItems;
        }

        /// <summary>
        /// Removes <paramref name="ribbonItem"/> from <paramref name="ribbonPanel"/>, including from list buttons and stacked rows.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <param name="ribbonItem">The item to remove.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        public static RibbonPanel Remove(this RibbonPanel ribbonPanel, RibbonItem ribbonItem)
        {
            var aRibbonItem = ribbonItem.ToAWRibbonItem();
            var aRibbonPanel = ribbonPanel.ToAWRibbonPanel();

            aRibbonPanel.Source.Items.Remove(aRibbonItem);

            foreach (var item in aRibbonPanel.Source.Items)
            {
                if (item is Autodesk.Windows.RibbonListButton ribbonListButton)
                    ribbonListButton.Items?.Remove(aRibbonItem);
                if (item is Autodesk.Windows.RibbonRowPanel ribbonRowPanel)
                    ribbonRowPanel.Items?.Remove(aRibbonItem);
            }

            return ribbonPanel;
        }

        /// <summary>
        /// Moves <paramref name="ribbonPanel"/> to position <paramref name="newIndex"/> inside its tab.
        /// </summary>
        /// <param name="ribbonPanel">The panel to move.</param>
        /// <param name="newIndex">The new position; values out of range are clamped to the first or last position.</param>
        public static void MoveRibbonPanel(this Autodesk.Windows.RibbonPanel ribbonPanel, int newIndex = 0)
        {
            var ribbonTab = ribbonPanel.Tab;
            var panels = ribbonTab.Panels;
            var length = panels.Count;
            if (length <= 1) return;

            var index = panels.IndexOf(ribbonPanel);
            if (index == -1) return;

            newIndex = Math.Max(0, Math.Min(length - 1, newIndex));
            panels.Move(panels.IndexOf(ribbonPanel), newIndex);
        }

        /// <summary>
        /// Checks whether <paramref name="ribbonPanel"/> is still contained in its tab.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <returns><c>true</c> if the tab contains the panel.</returns>
        private static bool IsTabContains(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.GetRibbonTab().Panels.Contains(ribbonPanel.ToAWRibbonPanel());
        }

        /// <summary>
        /// Checks whether <paramref name="ribbonPanel"/> is in its tab and its name ends with <paramref name="panelName"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <param name="panelName">The panel name to match.</param>
        /// <returns><c>true</c> if the panel matches.</returns>
        private static bool IsSelect(this RibbonPanel ribbonPanel, string panelName)
        {
            return ribbonPanel.IsTabContains() && ribbonPanel.Name.EndsWith(panelName);
        }
    }
}