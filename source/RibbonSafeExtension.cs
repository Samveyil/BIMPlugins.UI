using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    internal static class RibbonSafeExtension
    {
        internal static string SafeButtonName(string buttonName)
        {
            return $"{buttonName}_{TickNumber}";
        }

        internal static string SafeRibbonPanelName(string panelName)
        {
            return $"{System.DateTime.Now.Ticks + TickNumber}%{panelName}";
        }

        internal const string SafeMinimalText = "-";

        private static long _TickNumber;
        private static long TickNumber => _TickNumber++;

        internal static bool VerifyNameExclusive<T>(T ribbonItem, string name) where T : class
        {
            //var type = typeof(T);
            var type = ribbonItem.GetType();
            try
            {
                type.GetMethod("verifyNameExclusive",
                    BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.Invoke(ribbonItem, new[] { name });
                return false;
            }
            catch { }
            return true;
        }

        internal static string GenerateSafeButtonName<T>(T ribbonItem, string targetName, string targetText = null) where T : class
        {
            if (targetText == null)
                targetText = targetName;

            while (RibbonSafeExtension.VerifyNameExclusive(ribbonItem, targetName))
                targetName = RibbonSafeExtension.SafeButtonName(targetText);

            return targetName;
        }

        internal static PushButtonData NewPushButtonData(object ribbonItem, Type commandType, string text = null)
        {
            return NewPushButtonData<PushButtonData>(ribbonItem, commandType, text);
        }
        internal static T NewPushButtonData<T>(object ribbonItem, Type commandType, string text = null) where T : PushButtonData
        {
            var targetName = commandType.GetName();
            var targetText = targetName;
            var assemblyName = commandType.Assembly.Location;
            var className = commandType.FullName;

            if (!string.IsNullOrWhiteSpace(text))
                targetText = text;

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonItem, targetName, targetText);

            var buttonData = (T)Activator.CreateInstance(typeof(T), targetName, targetText, assemblyName, className);

            if (typeof(IExternalCommandAvailability).IsAssignableFrom(commandType))
                buttonData.AvailabilityClassName = commandType.FullName;

            if (string.IsNullOrWhiteSpace(text))
                buttonData.Text = targetName;

            return buttonData;
        }
        internal static PushButtonData NewPushButtonData<TExternalCommand>(object ribbonItem, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var commandType = typeof(TExternalCommand);
            return RibbonSafeExtension.NewPushButtonData(ribbonItem, commandType, text);
        }
        internal static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(object ribbonItem, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButtonData buttonData = RibbonSafeExtension
                .NewPushButtonData<TExternalCommand>(ribbonItem, text)
                .SetAvailability<TAvailability>();
            return buttonData;
        }
    }
}