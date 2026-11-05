using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to get the underlying <c>Autodesk.Windows</c> (AdWindows) objects of Revit API ribbon objects.
    /// </summary>
    public static class RibbonUtilExtension
    {
        /// <summary>
        /// Gets the <see cref="Autodesk.Windows.RibbonItem"/> behind <paramref name="ribbonItem"/> through reflection.
        /// </summary>
        /// <param name="ribbonItem">The Revit API ribbon item.</param>
        /// <returns>The AdWindows ribbon item, or <c>null</c> if it could not be found.</returns>
        public static Autodesk.Windows.RibbonItem ToAWRibbonItem(this RibbonItem ribbonItem)
        {
            var ribbonItemType = typeof(RibbonItem);
            var _ribbonItem = ribbonItemType.GetField("m_RibbonItem", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonItem) as Autodesk.Windows.RibbonItem;

            return _ribbonItem;
        }
        /// <summary>
        /// Gets the <see cref="Autodesk.Windows.RibbonItem"/> behind <paramref name="ribbonItem"/> as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the AdWindows ribbon item.</typeparam>
        /// <param name="ribbonItem">The Revit API ribbon item.</param>
        /// <returns>The AdWindows ribbon item, or <c>null</c> if it is not found or is not <typeparamref name="T"/>.</returns>
        public static T ToAWRibbonItem<T>(this RibbonItem ribbonItem) where T : Autodesk.Windows.RibbonItem
        {
            return ribbonItem.ToAWRibbonItem() as T;
        }
        /// <summary>
        /// Gets the <see cref="Autodesk.Windows.RibbonItem"/> behind <paramref name="ribbonItem"/> as <typeparamref name="T"/> and runs <paramref name="action"/> on it.
        /// </summary>
        /// <typeparam name="T">Type of the AdWindows ribbon item.</typeparam>
        /// <param name="ribbonItem">The Revit API ribbon item.</param>
        /// <param name="action">Action invoked with the AdWindows ribbon item if it is found.</param>
        /// <returns>The AdWindows ribbon item, or <c>null</c> if it is not found or is not <typeparamref name="T"/>.</returns>
        public static T ToAWRibbonItem<T>(this RibbonItem ribbonItem, Action<T> action) where T : Autodesk.Windows.RibbonItem
        {
            var ribbon = ribbonItem.ToAWRibbonItem<T>();
            if (ribbon is not null) action?.Invoke(ribbon);
            return ribbon;
        }

        /// <summary>
        /// Gets the <see cref="Autodesk.Windows.RibbonPanel"/> behind <paramref name="ribbonPanel"/> through reflection.
        /// </summary>
        /// <param name="ribbonPanel">The Revit API ribbon panel.</param>
        /// <returns>The AdWindows ribbon panel, or <c>null</c> if it could not be found.</returns>
        public static Autodesk.Windows.RibbonPanel ToAWRibbonPanel(this RibbonPanel ribbonPanel)
        {
            var type = typeof(RibbonPanel);
            var _ribbonPanel = type.GetField("m_RibbonPanel", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(ribbonPanel) as Autodesk.Windows.RibbonPanel;
            return _ribbonPanel;
        }
    }
}