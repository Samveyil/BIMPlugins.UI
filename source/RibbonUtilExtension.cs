using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    public static class RibbonUtilExtension
    {
        public static Autodesk.Windows.RibbonItem ToAWRibbonItem(this RibbonItem ribbonItem)
        {
            var ribbonItemType = typeof(RibbonItem);
            var _ribbonItem = ribbonItemType.GetField("m_RibbonItem", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonItem) as Autodesk.Windows.RibbonItem;

            return _ribbonItem;
        }
        public static T ToAWRibbonItem<T>(this RibbonItem ribbonItem) where T : Autodesk.Windows.RibbonItem
        {
            return ribbonItem.ToAWRibbonItem() as T;
        }
        public static T ToAWRibbonItem<T>(this RibbonItem ribbonItem, Action<T> action) where T : Autodesk.Windows.RibbonItem
        {
            var ribbon = ribbonItem.ToAWRibbonItem<T>();
            if (ribbon is not null) action?.Invoke(ribbon);
            return ribbon;
        }

        public static Autodesk.Windows.RibbonPanel ToAWRibbonPanel(this RibbonPanel ribbonPanel)
        {
            var type = typeof(RibbonPanel);
            var _ribbonPanel = type.GetField("m_RibbonPanel", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonPanel) as Autodesk.Windows.RibbonPanel;
            return _ribbonPanel;
        }
    }
}