using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace BIMPlugins.UI
{
    public static class RibbonToggleExtension
    {
        public static ToggleButton CreateToggleButton<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var toggleBtnData = ribbonPanel.NewToggleButtonData<TExternalCommand>(text);

            return ribbonPanel.CreateToggleButton(toggleBtnData);
        }
        public static ToggleButton CreateToggleButton<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            return ribbonPanel.CreateToggleButton<TExternalCommand>(text).SetAvailability<TAvailability>();
        }
        public static ToggleButton CreateToggleButton(this RibbonPanel ribbonPanel, ToggleButtonData toggleBtnData)
        {
            var awRibbonPanel = ribbonPanel.ToAWRibbonPanel();
            
            var toggleBtn = NewToggleButton(toggleBtnData, awRibbonPanel.Source.Id);

            awRibbonPanel.Source.Items.Add(toggleBtn.ToAWRibbonItem<Autodesk.Windows.RibbonToggleButton>());

            return toggleBtn;
        }

        private static ToggleButton NewToggleButton(ToggleButtonData toggleBtnData, string panelId)
        {
            MethodInfo method = typeof(ToggleButtonData).GetMethod("createToggleButton", BindingFlags.Static | BindingFlags.NonPublic);
            if (method != null)
                return (ToggleButton)method.Invoke(null, new object[] { toggleBtnData, panelId });
            return null;
        }

        public static ToggleButtonData NewToggleButtonData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ToggleButtonData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new ToggleButtonData(targetName, targetName);
        }
        public static ToggleButtonData NewToggleButtonData<TExternalCommand>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var commandType = typeof(TExternalCommand);
            return ribbonPanel.NewToggleButtonData(commandType, text);
        }
        public static ToggleButtonData NewToggleButtonData<TExternalCommand, TAvailability>(this RibbonPanel ribbonPanel, string text = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var buttonData = ribbonPanel.NewToggleButtonData<TExternalCommand>(text).SetAvailability<TAvailability>();
            return buttonData;
        }
        public static ToggleButtonData NewToggleButtonData(this RibbonPanel ribbonPanel, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData<ToggleButtonData>(ribbonPanel, commandType, text);
        }

        public static ToggleButtonData SetAvailability<TAvailability>(this ToggleButtonData toggleBtnData) where TAvailability : class, IExternalCommandAvailability, new()
        {
            toggleBtnData.AvailabilityClassName = typeof(TAvailability).FullName;
            return toggleBtnData;
        }
        public static ToggleButton SetAvailability<TAvailability>(this ToggleButton toggleButton) where TAvailability : class, IExternalCommandAvailability, new()
        {
            toggleButton.AvailabilityClassName = typeof(TAvailability).FullName;
            return toggleButton;
        }
    }
}