using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and fill <see cref="RadioButtonGroup"/>.
    /// </summary>
    public static class RibbonRadioExtension
    {
        /// <summary>
        /// Creates a <see cref="RadioButtonGroup"/> in <paramref name="ribbonPanel"/> containing <paramref name="toggleButtonDatas"/>.
        /// The name is taken from the text of the first button.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the group to.</param>
        /// <param name="toggleButtonDatas">The toggle buttons to add.</param>
        /// <returns>The created <see cref="RadioButtonGroup"/>.</returns>
        public static RadioButtonGroup CreateRadioButtonGroup(this RibbonPanel ribbonPanel, params ToggleButtonData[] toggleButtonDatas)
        {
            return ribbonPanel.CreateRadioButtonGroup(null, toggleButtonDatas);
        }
        /// <summary>
        /// Creates a <see cref="RadioButtonGroup"/> in <paramref name="ribbonPanel"/> named after <paramref name="targetText"/> and containing <paramref name="toggleButtonDatas"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the group to.</param>
        /// <param name="targetText">The group name. When empty, the text of the first button, or <c>"RadioButtonGroup"</c>, is used.</param>
        /// <param name="toggleButtonDatas">The toggle buttons to add.</param>
        /// <returns>The created <see cref="RadioButtonGroup"/>.</returns>
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

        /// <summary>
        /// Adds <paramref name="toggleButtonDatas"/> to <paramref name="radioButtonGroup"/>, renaming each button to a unique name.
        /// </summary>
        /// <param name="radioButtonGroup">The radio button group.</param>
        /// <param name="toggleButtonDatas">The toggle buttons to add.</param>
        /// <returns>The same <paramref name="radioButtonGroup"/>.</returns>
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

        /// <summary>
        /// Creates a new <see cref="RadioButtonGroupData"/> with a name unique in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="targetName">The base name. When empty, <c>"RadioButtonGroupData"</c> is used.</param>
        /// <returns>The new <see cref="RadioButtonGroupData"/>.</returns>
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