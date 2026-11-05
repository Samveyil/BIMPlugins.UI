using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and configure a standalone <see cref="ToggleButton"/> and <see cref="ToggleButtonData"/>.
    /// </summary>
    public static class RibbonToggleExtension
    {
        /// <summary>
        /// Creates a <see cref="ToggleButton"/> in <paramref name="ribbonPanel"/> that runs <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="ribbonPanel">The panel to add the button to.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="ToggleButton"/>.</returns>
        public static ToggleButton CreateToggleButton<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var toggleBtnData = ribbonPanel.NewToggleButtonData<TExternalCommand>(text);

            return ribbonPanel.CreateToggleButton(toggleBtnData);
        }
        /// <summary>
        /// Creates a <see cref="ToggleButton"/> in <paramref name="ribbonPanel"/> that runs <typeparamref name="TExternalCommand"/>
        /// and is enabled according to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="ribbonPanel">The panel to add the button to.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="ToggleButton"/>.</returns>
        public static ToggleButton CreateToggleButton<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return ribbonPanel.CreateToggleButton<TExternalCommand>(text).SetAvailability<TAvailability>();
        }
        /// <summary>
        /// Creates a <see cref="ToggleButton"/> from <paramref name="toggleBtnData"/> and adds it directly to <paramref name="ribbonPanel"/>,
        /// outside of a <see cref="RadioButtonGroup"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the button to.</param>
        /// <param name="toggleBtnData">The toggle button data.</param>
        /// <returns>The created <see cref="ToggleButton"/>.</returns>
        public static ToggleButton CreateToggleButton(this RibbonPanel ribbonPanel, ToggleButtonData toggleBtnData)
        {
            var awRibbonPanel = ribbonPanel.ToAWRibbonPanel();

            var toggleBtn = NewToggleButton(toggleBtnData, awRibbonPanel.Source.Id);

            awRibbonPanel.Source.Items.Add(toggleBtn.ToAWRibbonItem<Autodesk.Windows.RibbonToggleButton>());

            return toggleBtn;
        }

        /// <summary>
        /// Creates a <see cref="ToggleButton"/> by calling the internal Revit method <c>ToggleButtonData.createToggleButton</c>.
        /// </summary>
        /// <param name="toggleBtnData">The toggle button data.</param>
        /// <param name="panelId">The id of the panel that will contain the button.</param>
        /// <returns>The created <see cref="ToggleButton"/>, or <c>null</c> if the internal method is not found.</returns>
        private static ToggleButton NewToggleButton(ToggleButtonData toggleBtnData, string panelId)
        {
            MethodInfo method = typeof(ToggleButtonData).GetMethod("createToggleButton", BindingFlags.Static | BindingFlags.NonPublic);
            if (method != null)
                return (ToggleButton)method.Invoke(null, new object[] { toggleBtnData, panelId });
            return null;
        }

        /// <summary>
        /// Creates a new <see cref="ToggleButtonData"/> without a command, with a name unique in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="targetName">The button name and text. When empty, <c>"ToggleButtonData"</c> is used.</param>
        /// <returns>The new <see cref="ToggleButtonData"/>.</returns>
        public static ToggleButtonData NewToggleButtonData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ToggleButtonData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new ToggleButtonData(targetName, targetName);
        }
        /// <summary>
        /// Creates a new <see cref="ToggleButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="ToggleButtonData"/>.</returns>
        public static ToggleButtonData NewToggleButtonData<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var commandType = typeof(TExternalCommand);
            return ribbonPanel.NewToggleButtonData(commandType, text);
        }
        /// <summary>
        /// Creates a new <see cref="ToggleButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <typeparamref name="TExternalCommand"/>
        /// and availability <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="ToggleButtonData"/>.</returns>
        public static ToggleButtonData NewToggleButtonData<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var buttonData = ribbonPanel.NewToggleButtonData<TExternalCommand>(text).SetAvailability<TAvailability>();
            return buttonData;
        }
        /// <summary>
        /// Creates a new <see cref="ToggleButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <paramref name="commandType"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="ToggleButtonData"/>.</returns>
        public static ToggleButtonData NewToggleButtonData(this RibbonPanel ribbonPanel, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData<ToggleButtonData>(ribbonPanel, commandType, text);
        }

        /// <summary>
        /// Sets <see cref="PushButtonData.AvailabilityClassName"/> of <paramref name="toggleBtnData"/> to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="toggleBtnData">The toggle button data.</param>
        /// <returns>The same <paramref name="toggleBtnData"/>.</returns>
        public static ToggleButtonData SetAvailability<TAvailability>(this ToggleButtonData toggleBtnData) where TAvailability : class, IExternalCommandAvailability, new()
        {
            toggleBtnData.AvailabilityClassName = typeof(TAvailability).FullName;
            return toggleBtnData;
        }
        /// <summary>
        /// Sets <see cref="PushButton.AvailabilityClassName"/> of <paramref name="toggleButton"/> to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="toggleButton">The toggle button.</param>
        /// <returns>The same <paramref name="toggleButton"/>.</returns>
        public static ToggleButton SetAvailability<TAvailability>(this ToggleButton toggleButton) where TAvailability : class, IExternalCommandAvailability, new()
        {
            toggleButton.AvailabilityClassName = typeof(TAvailability).FullName;
            return toggleButton;
        }
    }
}