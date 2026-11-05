using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Helpers to generate unique names for ribbon panels and items, so that adding an item never fails because of a duplicate name.
    /// </summary>
    internal static class RibbonSafeExtension
    {
        /// <summary>
        /// Generates a unique button name by appending an incrementing number to <paramref name="buttonName"/>.
        /// </summary>
        /// <param name="buttonName">The base name.</param>
        /// <returns>The name in the format <c>{buttonName}_{number}</c>.</returns>
        internal static string SafeButtonName(string buttonName)
        {
            return $"{buttonName}_{TickNumber}";
        }

        /// <summary>
        /// Generates a unique panel name by prefixing <paramref name="panelName"/> with the current ticks.
        /// </summary>
        /// <param name="panelName">The base name.</param>
        /// <returns>The name in the format <c>{ticks}%{panelName}</c>.</returns>
        internal static string SafeRibbonPanelName(string panelName)
        {
            return $"{System.DateTime.Now.Ticks + TickNumber}%{panelName}";
        }

        /// <summary>
        /// Minimal text used when an item must have a non-empty text.
        /// </summary>
        internal const string SafeMinimalText = "-";

        private static long _TickNumber;
        /// <summary>
        /// Incrementing counter used to make generated names unique.
        /// </summary>
        private static long TickNumber => _TickNumber++;

        /// <summary>
        /// Checks whether <paramref name="name"/> is already used in <paramref name="ribbonItem"/>,
        /// by calling the internal Revit method <c>verifyNameExclusive</c>.
        /// </summary>
        /// <typeparam name="T">Type of the ribbon panel or item.</typeparam>
        /// <param name="ribbonItem">The ribbon panel or item that contains the new item.</param>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if the name is already used; <c>false</c> if it is free.</returns>
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

        /// <summary>
        /// Returns <paramref name="targetName"/> if it is free in <paramref name="ribbonItem"/>,
        /// otherwise generates new names from <paramref name="targetText"/> until a free one is found.
        /// </summary>
        /// <typeparam name="T">Type of the ribbon panel or item.</typeparam>
        /// <param name="ribbonItem">The ribbon panel or item that contains the new item.</param>
        /// <param name="targetName">The preferred name.</param>
        /// <param name="targetText">The base for generated names. When <c>null</c>, <paramref name="targetName"/> is used.</param>
        /// <returns>A name that is unique in <paramref name="ribbonItem"/>.</returns>
        internal static string GenerateSafeButtonName<T>(T ribbonItem, string targetName, string targetText = null) where T : class
        {
            if (targetText == null)
                targetText = targetName;

            while (RibbonSafeExtension.VerifyNameExclusive(ribbonItem, targetName))
                targetName = RibbonSafeExtension.SafeButtonName(targetText);

            return targetName;
        }

        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonItem"/> for <paramref name="commandType"/>.
        /// </summary>
        /// <param name="ribbonItem">The ribbon panel or item used to generate a unique name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        internal static PushButtonData NewPushButtonData(object ribbonItem, Type commandType, string text = null)
        {
            return NewPushButtonData<PushButtonData>(ribbonItem, commandType, text);
        }
        /// <summary>
        /// Creates a new button data of type <typeparamref name="T"/> with a name unique in <paramref name="ribbonItem"/> for <paramref name="commandType"/>.
        /// If <paramref name="commandType"/> also implements <see cref="IExternalCommandAvailability"/>, it is used as availability class.
        /// </summary>
        /// <typeparam name="T">Type of the button data, for example <see cref="PushButtonData"/> or <see cref="ToggleButtonData"/>.</typeparam>
        /// <param name="ribbonItem">The ribbon panel or item used to generate a unique name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the generated button name is used.</param>
        /// <returns>The new button data.</returns>
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
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonItem"/> for <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="ribbonItem">The ribbon panel or item used to generate a unique name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        internal static PushButtonData NewPushButtonData<TExternalCommand>(object ribbonItem, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var commandType = typeof(TExternalCommand);
            return RibbonSafeExtension.NewPushButtonData(ribbonItem, commandType, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonItem"/> for <typeparamref name="TExternalCommand"/>
        /// and availability <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="ribbonItem">The ribbon panel or item used to generate a unique name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        internal static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(object ribbonItem, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButtonData buttonData = RibbonSafeExtension
                .NewPushButtonData<TExternalCommand>(ribbonItem, text)
                .SetAvailability<TAvailability>();
            return buttonData;
        }
    }
}