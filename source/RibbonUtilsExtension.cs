using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    public static class RibbonUtilExtension
    {
        private static string GetId(this RibbonItem ribbonItem)
        {
            var type = typeof(RibbonItem);

            var parentId = type.GetField("m_parentId",
                BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonItem)
                ?? string.Empty;

            var generateIdMethod = type.GetMethod("generateId",
                BindingFlags.Static | BindingFlags.NonPublic);

            return (string)generateIdMethod?.Invoke(ribbonItem, new[] { parentId, ribbonItem.Name });
        }

        public static Autodesk.Windows.RibbonItem ToAWRibbonItem(this RibbonItem ribbonItem)
        {
            var ribbonItemType = typeof(RibbonItem);

            var buttonField = ribbonItemType.GetField("m_RibbonItem",
                BindingFlags.Instance | BindingFlags.NonPublic);

            return buttonField?.GetValue(ribbonItem) as Autodesk.Windows.RibbonItem;
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
            var _ribbonPanel = type.GetField("m_RibbonPanel",
                BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonPanel) as Autodesk.Windows.RibbonPanel;
            return _ribbonPanel;
        }
    }
}