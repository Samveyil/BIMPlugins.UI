using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonSplitExtension
    {
        public static SplitButton CreateSplitButton(this RibbonPanel ribbonPanel, params PushButtonData[] pushButtons)
        {
            return ribbonPanel.CreateSplitButton(null, pushButtons);
        }
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

        public static PushButton CreatePushButton<TExternalCommand>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = splitButton.AddPushButton(splitButton.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = splitButton
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        public static PushButtonData NewPushButtonData(this SplitButton splitButton, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(splitButton, commandType, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(splitButton, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this SplitButton splitButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(splitButton, text);
        }
    }
}