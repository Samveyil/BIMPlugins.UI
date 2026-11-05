using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and fill <see cref="PulldownButton"/>.
    /// </summary>
    public static class RibbonPulldownExtension
    {
        /// <summary>
        /// Creates a <see cref="PulldownButton"/> in <paramref name="ribbonPanel"/> containing <paramref name="pushButtons"/>.
        /// The text is taken from the first button.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the pulldown button to.</param>
        /// <param name="pushButtons">The buttons to add to the drop-down list.</param>
        /// <returns>The created <see cref="PulldownButton"/>.</returns>
        public static PulldownButton CreatePulldownButton(this RibbonPanel ribbonPanel, params PushButtonData[] pushButtons)
        {
            return ribbonPanel.CreatePulldownButton(null, pushButtons);
        }
        /// <summary>
        /// Creates a <see cref="PulldownButton"/> in <paramref name="ribbonPanel"/> with text <paramref name="targetText"/> containing <paramref name="pushButtons"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the pulldown button to.</param>
        /// <param name="targetText">The pulldown button text. When empty, the text of the first button, or <c>"PulldownButton"</c>, is used.</param>
        /// <param name="pushButtons">The buttons to add to the drop-down list.</param>
        /// <returns>The created <see cref="PulldownButton"/>.</returns>
        public static PulldownButton CreatePulldownButton(this RibbonPanel ribbonPanel, string targetText, params PushButtonData[] pushButtons)
        {
            PulldownButton pulldownButton = null;

            if (string.IsNullOrWhiteSpace(targetText))
                targetText = pushButtons.FirstOrDefault()?.Text ?? nameof(PulldownButton);

            var targetName = targetText;

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetText);

            pulldownButton = ribbonPanel.AddItem(new PulldownButtonData(targetName, targetText)) as PulldownButton;

            pulldownButton.AddPushButtons(pushButtons);

            return pulldownButton;
        }

        /// <summary>
        /// Adds <paramref name="pushButtons"/> to <paramref name="pulldownButton"/>, renaming each button to a unique name.
        /// </summary>
        /// <typeparam name="T">Type of the pulldown button.</typeparam>
        /// <param name="pulldownButton">The pulldown or split button.</param>
        /// <param name="pushButtons">The buttons to add.</param>
        /// <returns>The same <paramref name="pulldownButton"/>.</returns>
        public static T AddPushButtons<T>(this T pulldownButton, params PushButtonData[] pushButtons) where T : PulldownButton
        {
            var targetText = pulldownButton.ItemText;
            foreach (PushButtonData pushButton in pushButtons)
            {
                pushButton.Name = RibbonSafeExtension.GenerateSafeButtonName(pulldownButton, pushButton.Name, targetText);

                pulldownButton.AddPushButton(pushButton);
            }
            return pulldownButton;
        }

        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="pulldownButton"/> that runs <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="pulldownButton">The pulldown or split button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = pulldownButton.AddPushButton(pulldownButton.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        /// <summary>
        /// Creates a <see cref="PushButton"/> in <paramref name="pulldownButton"/> that runs <typeparamref name="TExternalCommand"/>
        /// and is enabled according to <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="pulldownButton">The pulldown or split button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The created <see cref="PushButton"/>.</returns>
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = pulldownButton
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="pulldownButton"/> for <paramref name="commandType"/>.
        /// </summary>
        /// <param name="pulldownButton">The pulldown button used to generate a unique button name.</param>
        /// <param name="commandType">The <see cref="IExternalCommand"/> type executed by the button.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData(this PulldownButton pulldownButton, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(pulldownButton, commandType, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="pulldownButton"/> for <typeparamref name="TExternalCommand"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <param name="pulldownButton">The pulldown button used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(pulldownButton, text);
        }
        /// <summary>
        /// Creates a new <see cref="PushButtonData"/> with a name unique in <paramref name="pulldownButton"/> for <typeparamref name="TExternalCommand"/>
        /// and availability <typeparamref name="TAvailability"/>.
        /// </summary>
        /// <typeparam name="TExternalCommand">The external command executed by the button.</typeparam>
        /// <typeparam name="TAvailability">The class that decides when the button is enabled.</typeparam>
        /// <param name="pulldownButton">The pulldown button used to generate a unique button name.</param>
        /// <param name="text">The button text. When <c>null</c>, the command type name is used.</param>
        /// <returns>The new <see cref="PushButtonData"/>.</returns>
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(pulldownButton, text);
        }
    }
}