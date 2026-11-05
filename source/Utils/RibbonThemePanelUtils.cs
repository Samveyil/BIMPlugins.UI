using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Keeps the list of panels whose item images are updated when the Revit theme changes.
    /// </summary>
    internal static class RibbonThemePanelUtils
    {
        private static List<RibbonPanel> RibbonThemePanels = new List<RibbonPanel>();
        static RibbonThemePanelUtils()
        {
            RibbonThemeUtils.ThemeChanged += RibbonThemeUtils_ThemeChanged;
        }
        /// <summary>
        /// Unsubscribes from <see cref="RibbonThemeUtils.ThemeChanged"/> and clears the list of panels.
        /// </summary>
        internal static void Dispose()
        {
            RibbonThemeUtils.ThemeChanged -= RibbonThemeUtils_ThemeChanged;
            RibbonThemePanels.Clear();
        }
        /// <summary>
        /// Stops updating the images of <paramref name="ribbonPanel"/> when the theme changes.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        internal static RibbonPanel ThemeChangeDisable(this RibbonPanel ribbonPanel) => ThemeChangeEnable(ribbonPanel, false);
        /// <summary>
        /// Starts or stops updating the images of <paramref name="ribbonPanel"/> when the theme changes.
        /// </summary>
        /// <param name="ribbonPanel">The panel.</param>
        /// <param name="enable"><c>true</c> to update the images on theme change; <c>false</c> to stop.</param>
        /// <returns>The same <paramref name="ribbonPanel"/>.</returns>
        internal static RibbonPanel ThemeChangeEnable(this RibbonPanel ribbonPanel, bool enable = true)
        {
            if (ribbonPanel is null) return ribbonPanel;

            RibbonThemePanels.Remove(ribbonPanel);

            if (enable)
            {
                RibbonThemePanels.Add(ribbonPanel);
            }

            return ribbonPanel;
        }

        /// <summary>
        /// Updates the images of all items of the registered panels to the new theme.
        /// </summary>
        private static void RibbonThemeUtils_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            foreach (var ribbonPanel in RibbonThemePanels)
            {
                foreach (var ribbonItem in ribbonPanel.GetRibbonItems())
                {
                    var isLight = e.IsLight;
                    UpdateImageThemes(ribbonItem, isLight);
                }
            }
        }

        /// <summary>
        /// Sets the images of <paramref name="ribbonItem"/> again, so they are replaced with the variants for the current theme.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="isLight"><c>true</c> if the current theme is light.</param>
        private static void UpdateImageThemes(RibbonItem ribbonItem, bool isLight)
        {
            try
            {
                switch (ribbonItem)
                {
                    case RibbonButton ribbonButton:
                        var image = ribbonButton.Image;
                        ribbonButton.SetLargeImage(ribbonButton.LargeImage);
                        if (image is not null) ribbonButton.SetImage(image);
                        break;
                    case ComboBox comboBox:
                        comboBox.SetImage(comboBox.Image);
                        break;
                    case ComboBoxMember comboBoxMember:
                        comboBoxMember.SetImage(comboBoxMember.Image);
                        break;
                    case TextBox textBox:
                        textBox.SetImage(textBox.Image);
                        break;
                }
            }
            catch { }
        }
    }
}