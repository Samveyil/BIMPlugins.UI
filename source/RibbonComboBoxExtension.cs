using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to create and configure <see cref="ComboBox"/>, <see cref="ComboBoxMember"/> and their data.
    /// </summary>
    public static class RibbonComboBoxExtension
    {
        /// <summary>
        /// Creates a <see cref="ComboBox"/> in <paramref name="ribbonPanel"/> filled with <paramref name="comboBoxMemberDatas"/>.
        /// The name is taken from the text of the first member.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the combo box to.</param>
        /// <param name="comboBoxMemberDatas">The members to add.</param>
        /// <returns>The created <see cref="ComboBox"/>.</returns>
        public static ComboBox CreateComboBox(this RibbonPanel ribbonPanel, params ComboBoxMemberData[] comboBoxMemberDatas)
        {
            return ribbonPanel.CreateComboBox(null, comboBoxMemberDatas);
        }
        /// <summary>
        /// Creates a <see cref="ComboBox"/> in <paramref name="ribbonPanel"/> named after <paramref name="targetText"/> and filled with <paramref name="comboBoxMemberDatas"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel to add the combo box to.</param>
        /// <param name="targetText">The combo box name. When empty, the text of the first member, or <c>"ComboBox"</c>, is used.</param>
        /// <param name="comboBoxMemberDatas">The members to add.</param>
        /// <returns>The created <see cref="ComboBox"/>.</returns>
        public static ComboBox CreateComboBox(this RibbonPanel ribbonPanel, string targetText, params ComboBoxMemberData[] comboBoxMemberDatas)
        {
            ComboBox comboBox = null;

            if (string.IsNullOrWhiteSpace(targetText))
                targetText = comboBoxMemberDatas.FirstOrDefault()?.Text ?? nameof(ComboBox);

            var targetName = targetText;

            comboBox = ribbonPanel.AddItem(ribbonPanel.NewComboBoxData(targetName)) as ComboBox;

            comboBox.AddComboBoxMembers(comboBoxMemberDatas);

            return comboBox;
        }

        /// <summary>
        /// Adds <paramref name="comboBoxMemberDatas"/> to <paramref name="comboBox"/>, renaming each member to a unique name.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="comboBoxMemberDatas">The members to add.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox AddComboBoxMembers(this ComboBox comboBox, params ComboBoxMemberData[] comboBoxMemberDatas)
        {
            foreach (var comboBoxMemberData in comboBoxMemberDatas)
            {
                comboBoxMemberData.Name = RibbonSafeExtension.GenerateSafeButtonName(comboBox, comboBoxMemberData.Name, comboBox.Name);

                comboBox.AddItem(comboBoxMemberData);
            }
            return comboBox;
        }

        /// <summary>
        /// Sets the width of <paramref name="comboBox"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="width">The new width.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox SetWidth(this ComboBox comboBox, double width)
        {
            comboBox.ToAWRibbonItem().Width = width;
            return comboBox;
        }

        /// <summary>
        /// Sets the selected member of <paramref name="comboBox"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="current">The member to select.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox SetCurrent(this ComboBox comboBox, ComboBoxMember current)
        {
            comboBox.Current = current;
            return comboBox;
        }

        /// <summary>
        /// Subscribes <paramref name="currentChanged"/> to <see cref="ComboBox.CurrentChanged"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="currentChanged">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox AddCurrentChanged(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs> currentChanged)
        {
            comboBox.CurrentChanged += currentChanged;
            return comboBox;
        }
        /// <summary>
        /// Unsubscribes <paramref name="currentChanged"/> from <see cref="ComboBox.CurrentChanged"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="currentChanged">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox RemoveCurrentChanged(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs> currentChanged)
        {
            comboBox.CurrentChanged -= currentChanged;
            return comboBox;
        }

        /// <summary>
        /// Subscribes <paramref name="dropDownOpened"/> to <see cref="ComboBox.DropDownOpened"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="dropDownOpened">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox AddDropDownOpened(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownOpenedEventArgs> dropDownOpened)
        {
            comboBox.DropDownOpened += dropDownOpened;
            return comboBox;
        }
        /// <summary>
        /// Unsubscribes <paramref name="dropDownOpened"/> from <see cref="ComboBox.DropDownOpened"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="dropDownOpened">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox RemoveDropDownOpened(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownOpenedEventArgs> dropDownOpened)
        {
            comboBox.DropDownOpened -= dropDownOpened;
            return comboBox;
        }

        /// <summary>
        /// Subscribes <paramref name="dropDownClosed"/> to <see cref="ComboBox.DropDownClosed"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="dropDownClosed">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox AddDropDownClosed(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs> dropDownClosed)
        {
            comboBox.DropDownClosed += dropDownClosed;
            return comboBox;
        }
        /// <summary>
        /// Unsubscribes <paramref name="dropDownClosed"/> from <see cref="ComboBox.DropDownClosed"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="dropDownClosed">The event handler.</param>
        /// <returns>The same <paramref name="comboBox"/>.</returns>
        public static ComboBox RemoveDropDownClosed(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs> dropDownClosed)
        {
            comboBox.DropDownClosed -= dropDownClosed;
            return comboBox;
        }

        /// <summary>
        /// Creates a <see cref="ComboBoxMember"/> with a unique name and adds it to <paramref name="comboBox"/>.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="targetName">The member name and text. When empty, <c>"ComboBoxMemberData"</c> is used.</param>
        /// <param name="groupName">Optional group the member belongs to.</param>
        /// <returns>The created <see cref="ComboBoxMember"/>.</returns>
        public static ComboBoxMember CreateComboBoxMember(this ComboBox comboBox, string targetName = null, string groupName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ComboBoxMemberData);

            var comboBoxMemberData = new ComboBoxMemberData(targetName, targetName);

            if (!string.IsNullOrWhiteSpace(groupName))
                comboBoxMemberData.SetGroupName(groupName);

            var targetText = targetName;

            comboBoxMemberData.Name = RibbonSafeExtension.GenerateSafeButtonName(comboBox, comboBoxMemberData.Name, targetText);

            return comboBox.AddItem(comboBoxMemberData);
        }

        /// <summary>
        /// Creates a new <see cref="ComboBoxData"/> with a name unique in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="targetName">The base name. When empty, <c>"ComboBoxData"</c> is used.</param>
        /// <returns>The new <see cref="ComboBoxData"/>.</returns>
        public static ComboBoxData NewComboBoxData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ComboBoxData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new ComboBoxData(targetName);
        }

        /// <summary>
        /// Sets the group name of <paramref name="comboBoxMemberData"/>.
        /// </summary>
        /// <param name="comboBoxMemberData">The member data.</param>
        /// <param name="groupName">The group the member belongs to.</param>
        /// <returns>The same <paramref name="comboBoxMemberData"/>.</returns>
        public static ComboBoxMemberData SetGroupName(this ComboBoxMemberData comboBoxMemberData, string groupName)
        {
            comboBoxMemberData.GroupName = groupName;
            return comboBoxMemberData;
        }

        /// <summary>
        /// Creates a new <see cref="ComboBoxMemberData"/> with a name unique in <paramref name="ribbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The panel used to generate a unique name.</param>
        /// <param name="targetName">The member name and text. When empty, <c>"ComboBoxMemberData"</c> is used.</param>
        /// <returns>The new <see cref="ComboBoxMemberData"/>.</returns>
        public static ComboBoxMemberData NewComboBoxMemberData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ComboBoxMemberData);

            var comboBoxMemberData = new ComboBoxMemberData(targetName, targetName);

            var targetText = targetName;

            comboBoxMemberData.Name = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, comboBoxMemberData.Name, targetText);

            return comboBoxMemberData;
        }
    }
}