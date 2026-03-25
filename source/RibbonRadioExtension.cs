using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonRadioExtension
    {
        public static RadioButtonGroup CreateRadioButtonGroup(this RibbonPanel ribbonPanel, params ToggleButtonData[] toggleButtonDatas)
        {
            return ribbonPanel.CreateRadioButtonGroup(null, toggleButtonDatas);
        }
        public static RadioButtonGroup CreateRadioButtonGroup(this RibbonPanel ribbonPanel, string targetText, params ToggleButtonData[] toggleButtonDatas)
        {
            RadioButtonGroup radioButtonGroup = null;

            if (string.IsNullOrWhiteSpace(targetText))
                targetText = toggleButtonDatas.FirstOrDefault()?.Text ?? nameof(RadioButtonGroup);

            var targetName = targetText;

            radioButtonGroup = ribbonPanel.AddItem(ribbonPanel.NewRadioButtonGroupData(targetName)) as RadioButtonGroup;

            radioButtonGroup.AddToggleButtons(toggleButtonDatas);

            return radioButtonGroup;
        }

        public static RadioButtonGroup AddToggleButtons(this RadioButtonGroup radioButtonGroup, params ToggleButtonData[] toggleButtonDatas)
        {
            foreach (var toggleButtonData in toggleButtonDatas)
            {
                var targetText = toggleButtonData.Name;

                toggleButtonData.Name = RibbonSafeExtension.GenerateSafeButtonName(radioButtonGroup, toggleButtonData.Name, targetText);

                radioButtonGroup.AddItem(toggleButtonData);
            }
            return radioButtonGroup;
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
            var buttonData = ribbonPanel.NewToggleButtonData<TExternalCommand>(text);
            buttonData.AvailabilityClassName = typeof(TAvailability).FullName;
            return buttonData;
        }
        public static ToggleButtonData NewToggleButtonData(this RibbonPanel ribbonPanel, Type commandType, string text = null)
        {
            return RibbonSafeExtension.NewPushButtonData<ToggleButtonData>(ribbonPanel, commandType, text);
        }

        public static RadioButtonGroupData NewRadioButtonGroupData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(RadioButtonGroupData);

            var radioButtonGroupData = new RadioButtonGroupData(targetName);

            radioButtonGroupData.Name = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, radioButtonGroupData.Name, targetName);

            return radioButtonGroupData;
        }
    }
}