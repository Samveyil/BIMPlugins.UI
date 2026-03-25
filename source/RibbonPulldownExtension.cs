using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonPulldownExtension
    {
        public static PulldownButton CreatePulldownButton(this RibbonPanel ribbonPanel, params PushButtonData[] pushButtons)
        {
            return ribbonPanel.CreatePulldownButton(null, pushButtons);
        }
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

        public static PushButton CreatePushButton<TExternalCommand>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = pulldownButton.AddPushButton(pulldownButton.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = pulldownButton
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        public static PushButtonData NewPushButtonData(this PulldownButton pulldownButton, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(pulldownButton, commandType, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(pulldownButton, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this PulldownButton pulldownButton, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(pulldownButton, text);
        }
    }
}