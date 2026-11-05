using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and fill <see cref="SplitButton"/>.
    /// </summary>
    public static class RibbonSplitExtension
    {
        /// <summary>
        /// Creates a <see cref="SplitButton"/> in <paramref name="ribbonPanel"/> containing <paramref name="pushButtons"/>.
        /// The text is taken from the first button.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the split button to.</param>
        /// <param name="pushButtons">The buttons to add.</param>
        /// <returns>The created <see cref="SplitButton"/>.</returns>
        public static SplitButton CreateSplitButton(this RibbonPanel ribbonPanel, params PushButtonData[] pushButtons)
        {
            return ribbonPanel.CreateSplitButton(null, pushButtons);
        }
        /// <summary>
        /// Creates a <see cref="SplitButton"/> in <paramref name="ribbonPanel"/> with text <paramref name="targetText"/> containing <paramref name="pushButtons"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the split button to.</param>
        /// <param name="targetText">The split button text. When empty, the text of the first button, or <c>"SplitButton"</c>, is used.</param>
        /// <param name="pushButtons">The buttons to add.</param>
        /// <returns>The created <see cref="SplitButton"/>.</returns>
        public static SplitButton CreateSplitButton(this RibbonPanel ribbonPanel, string targetText, params PushButtonData[] pushButtons)
        {
            SplitButton splitButton = null;

            if (string.IsNullOrWhiteSpace(targetText))
                targetText = pushButtons.FirstOrDefault()?.Text ?? nameof(SplitButton);

            var targetName = targetText;

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetText);

            splitButton = ribbonPanel.AddItem(new SplitButtonData(targetName, targetText)) as SplitButton;

            splitButton.AddPushButtons(pushButtons);

            return splitButton;
        }

        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="splitButton"/> that runs <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="splitButton">The split button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = splitButton.AddPushButton(splitButton.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="splitButton"/> that runs <typeparamref name="TExternalCommand"/>
        /// and is enabled according to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="splitButton">The split button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = splitButton
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="splitButton"/> for <paramref name="commandType"/>.
        /// </summary>
        /// <param name="splitButton">The split button used to generate a unique button name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData(this SplitButton splitButton, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(splitButton, commandType, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="splitButton"/> for <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="splitButton">The split button used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(splitButton, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="splitButton"/> for <typeparamref name="TExternalCommand"/>
        /// and availability <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="splitButton">The split button used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(splitButton, text);
        }
    }
}