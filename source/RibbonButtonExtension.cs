using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and configure <see cref="PushButton"/> and <see cref="PushButtonData"/>.
    /// </summary>
    public static class RibbonButtonExtension
    {
        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="ribbonPanel"/> that runs <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="ribbonPanel">The panel to add the button to.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = ribbonPanel.AddItem(ribbonPanel.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="ribbonPanel"/> that runs <typeparamref name="TExternalCommand"/>
        /// and is enabled according to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="ribbonPanel">The panel to add the button to.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = ribbonPanel
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <paramref name="commandType"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique button name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData(this RibbonPanel ribbonPanel, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(ribbonPanel, commandType, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="ribbonPanel">The panel used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(ribbonPanel, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="ribbonPanel"/> for <typeparamref name="TExternalCommand"/>
        /// and availability <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="ribbonPanel">The panel used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(ribbonPanel, text);
        }

        /// <summary>
        /// Sets <see cref="PushButtonData.AvailabilityClassName"/> to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="pushButtonData">The button data.</param>
        /// <returns>The same <paramref name="pushButtonData"/>.</returns>
        public static PushButtonData SetAvailability<TAvailability>(this PushButtonData pushButtonData) where TAvailability : class, IExternalCommandAvailability, new()
        {
            pushButtonData.AvailabilityClassName = typeof(TAvailability).FullName;
            return pushButtonData;
        }
        /// <summary>
        /// Sets <see cref="PushButton.AvailabilityClassName"/> to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="pushButton">The button.</param>
        /// <returns>The same <paramref name="pushButton"/>.</returns>
        public static PushButton SetAvailability<TAvailability>(this PushButton pushButton) where TAvailability : class, IExternalCommandAvailability, new()
        {
            pushButton.AvailabilityClassName = typeof(TAvailability).FullName;
            return pushButton;
        }
    }
}