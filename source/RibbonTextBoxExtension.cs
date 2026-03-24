using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.UI
{
    public static class RibbonTextBoxExtension
    {
        public static TextBox CreateTextBox(this RibbonPanel ribbonPanel, string targetName = null)
        {
            return ribbonPanel.AddItem(ribbonPanel.NewTextBoxData(targetName)) as TextBox;
        }

        public static TextBox SetShowImageAsButton(this TextBox textBox, bool showImageAsButton = true)
        {
            textBox.ShowImageAsButton = showImageAsButton;
            return textBox;
        }

        public static TextBox SetSelectTextOnFocus(this TextBox textBox, bool selectTextOnFocus = true)
        {
            textBox.SelectTextOnFocus = selectTextOnFocus;
            return textBox;
        }

        public static TextBox SetPromptText(this TextBox textBox, string promptText)
        {
            textBox.PromptText = promptText;
            return textBox;
        }

        public static TextBox SetValue(this TextBox textBox, object value)
        {
            textBox.Value = value;
            return textBox;
        }

        public static TextBox SetWidth(this TextBox textBox, double width)
        {
            textBox.Width = width;
            return textBox;
        }

        public static TextBox AddEnterPressed(this TextBox textBox, EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs> enterPressed)
        {
            textBox.EnterPressed += enterPressed;
            return textBox;
        }
        public static TextBox RemoveEnterPressed(this TextBox textBox, EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs> enterPressed)
        {
            textBox.EnterPressed -= enterPressed;
            return textBox;
        }

        public static TextBoxData NewTextBoxData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(TextBoxData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new TextBoxData(targetName);
        }
    }
}