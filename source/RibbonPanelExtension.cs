using Autodesk.Revit.UI;
using BIMPlugins.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonPanelExtension
    {
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
        public static RibbonPanel CreateOrSelectPanel(this UIControlledApplication application, string panelName)
        {
            if (application.GetRibbonPanels().FirstOrDefault(p => p.IsSelect(panelName)) is RibbonPanel ribbonPanel)
                return ribbonPanel;

            return application.CreatePanel(panelName);
        }

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
        public static RibbonPanel CreateOrSelectPanel(this UIControlledApplication application, string tabName, string panelName)
        {
            if (string.IsNullOrEmpty(tabName))
                return application.CreateOrSelectPanel(panelName);

            if (application.GetRibbonPanels(tabName).FirstOrDefault(p => p.IsSelect(panelName)) is RibbonPanel ribbonPanel)
                return ribbonPanel;

            return application.CreatePanel(tabName, panelName);
        }

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

        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.MoveToRibbonTab("Modify");
        }
        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel, string ribbonTabId)
        {
            var ribbonTab = Autodesk.Windows.ComponentManager.Ribbon.FindTab(ribbonTabId);
            return ribbonPanel.MoveToRibbonTab(ribbonTab);
        }
        public static RibbonPanel MoveToRibbonTab(this RibbonPanel ribbonPanel, Autodesk.Windows.RibbonTab ribbonTab)
        {
            if (ribbonTab is not null)
            {
                var panel = ribbonPanel.ToAWRibbonPanel();
                ribbonTab.MoveToRibbonTab(panel);
            }
            return ribbonPanel;
        }

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

        private static bool IsTabContains(this RibbonPanel ribbonPanel)
        {
            return ribbonPanel.GetRibbonTab().Panels.Contains(ribbonPanel.ToAWRibbonPanel());
        }

        private static bool IsSelect(this RibbonPanel ribbonPanel, string panelName)
        {
            return ribbonPanel.IsTabContains() && ribbonPanel.Name.EndsWith(panelName);
        }
    }
}