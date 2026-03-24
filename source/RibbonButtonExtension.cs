using Autodesk.Revit.UI;
using System;

namespace BIMPlugins.UI
{
    public static class RibbonButtonExtension
    {
        public static PushButton CreatePushButton<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            PushButton pushButton = ribbonPanel.AddItem(ribbonPanel.NewPushButtonData<TExternalCommand>(text)) as PushButton;
            return pushButton;
        }
        public static PushButton CreatePushButton<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            PushButton pushButton = ribbonPanel
                .CreatePushButton<TExternalCommand>(text)
                .SetAvailability<TAvailability>();

            return pushButton;
        }

        public static PushButtonData NewPushButtonData(this RibbonPanel ribbonPanel, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData(ribbonPanel, commandType, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand>(ribbonPanel, text);
        }
        public static PushButtonData NewPushButtonData<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return RibbonSafeExtension.NewPushButtonData<TExternalCommand, TAvailability>(ribbonPanel, text);
        }

        public static PushButtonData SetAvailability<TAvailability>(this PushButtonData pushButtonData) where TAvailability : class, IExternalCommandAvailability, new()
        {
            pushButtonData.AvailabilityClassName = typeof(TAvailability).FullName;
            return pushButtonData;
        }
        public static PushButton SetAvailability<TAvailability>(this PushButton pushButton) where TAvailability : class, IExternalCommandAvailability, new()
        {
            pushButton.AvailabilityClassName = typeof(TAvailability).FullName;
            return pushButton;
        }
    }
}