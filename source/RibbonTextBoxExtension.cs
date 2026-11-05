using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and configure <see cref="TextBox"/>.
    /// </summary>
    public static class RibbonTextBoxExtension
    {
        /// <summary>
        /// Creates a <see cref="TextBox"/> with a unique name in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the text box to.</param>
        /// <param name="targetName">The base name. When empty, <c>"TextBoxData"</c> is used.</param>
        /// <returns>The created <see cref="TextBox"/>.</returns>
        public static TextBox CreateTextBox(this RibbonPanel ribbonPanel, string targetName = null)
        {
            return ribbonPanel.AddItem(ribbonPanel.NewTextBoxData(targetName)) as TextBox;
        }

        /// <summary>
        /// Sets whether the image of <paramref name="textBox"/> is shown as a clickable button.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="showImageAsButton"><c>true</c> to show the image as a button.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox SetShowImageAsButton(this TextBox textBox, bool showImageAsButton = true)
        {
            textBox.ShowImageAsButton = showImageAsButton;
            return textBox;
        }

        /// <summary>
        /// Sets whether the text of <paramref name="textBox"/> is selected when it gets focus.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="selectTextOnFocus"><c>true</c> to select the text on focus.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox SetSelectTextOnFocus(this TextBox textBox, bool selectTextOnFocus = true)
        {
            textBox.SelectTextOnFocus = selectTextOnFocus;
            return textBox;
        }

        /// <summary>
        /// Sets the hint text shown in <paramref name="textBox"/> when it is empty.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="promptText">The hint text.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox SetPromptText(this TextBox textBox, string promptText)
        {
            textBox.PromptText = promptText;
            return textBox;
        }

        /// <summary>
        /// Sets the value of <paramref name="textBox"/>.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="value">The value.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox SetValue(this TextBox textBox, object value)
        {
            textBox.Value = value;
            return textBox;
        }

        /// <summary>
        /// Sets the width of <paramref name="textBox"/>.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="width">The new width.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox SetWidth(this TextBox textBox, double width)
        {
            textBox.Width = width;
            return textBox;
        }

        /// <summary>
        /// Subscribes <paramref name="enterPressed"/> to <see cref="TextBox.EnterPressed"/>.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="enterPressed">The event handler.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox AddEnterPressed(this TextBox textBox, EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs> enterPressed)
        {
            textBox.EnterPressed += enterPressed;
            return textBox;
        }
        /// <summary>
        /// Unsubscribes <paramref name="enterPressed"/> from <see cref="TextBox.EnterPressed"/>.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="enterPressed">The event handler.</param>
        /// <returns>The same <paramref name="textBox"/>.</returns>
        public static TextBox RemoveEnterPressed(this TextBox textBox, EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs> enterPressed)
        {
            textBox.EnterPressed -= enterPressed;
            return textBox;
        }

        /// <summary>
        /// Creates a new <see cref="TextBoxData"/> with a name unique in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="targetName">The base name. When empty, <c>"TextBoxData"</c> is used.</param>
        /// <returns>The new <see cref="TextBoxData"/>.</returns>
        public static TextBoxData NewTextBoxData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(TextBoxData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new TextBoxData(targetName);
        }
    }
}