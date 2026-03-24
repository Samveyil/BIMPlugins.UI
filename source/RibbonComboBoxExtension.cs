using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace BIMPlugins.UI
{
    public static class RibbonComboBoxExtension
    {
        public static ComboBox CreateComboBox(this RibbonPanel ribbonPanel, params ComboBoxMemberData[] comboBoxMemberDatas)
        {
            return ribbonPanel.CreateComboBox(null, comboBoxMemberDatas);
        }
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

        public static ComboBox AddComboBoxMembers(this ComboBox comboBox, params ComboBoxMemberData[] comboBoxMemberDatas)
        {
            foreach (var comboBoxMemberData in comboBoxMemberDatas)
            {
                comboBoxMemberData.Name = RibbonSafeExtension.GenerateSafeButtonName(comboBox, comboBoxMemberData.Name, comboBox.Name);

                comboBox.AddItem(comboBoxMemberData);
            }
            return comboBox;
        }

        public static ComboBox SetWidth(this ComboBox comboBox, double width)
        {
            comboBox.ToAWRibbonItem().Width = width;
            return comboBox;
        }

        public static ComboBox SetCurrent(this ComboBox comboBox, ComboBoxMember current)
        {
            comboBox.Current = current;
            return comboBox;
        }

        public static ComboBox AddCurrentChanged(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs> currentChanged)
        {
            comboBox.CurrentChanged += currentChanged;
            return comboBox;
        }

        public static ComboBox RemoveCurrentChanged(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs> currentChanged)
        {
            comboBox.CurrentChanged -= currentChanged;
            return comboBox;
        }

        public static ComboBox AddDropDownOpened(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownOpenedEventArgs> dropDownOpened)
        {
            comboBox.DropDownOpened += dropDownOpened;
            return comboBox;
        }

        public static ComboBox RemoveDropDownOpened(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownOpenedEventArgs> dropDownOpened)
        {
            comboBox.DropDownOpened -= dropDownOpened;
            return comboBox;
        }

        public static ComboBox AddDropDownClosed(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs> dropDownClosed)
        {
            comboBox.DropDownClosed += dropDownClosed;
            return comboBox;
        }

        public static ComboBox RemoveDropDownClosed(this ComboBox comboBox, EventHandler<Autodesk.Revit.UI.Events.ComboBoxDropDownClosedEventArgs> dropDownClosed)
        {
            comboBox.DropDownClosed -= dropDownClosed;
            return comboBox;
        }

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

        public static ComboBoxData NewComboBoxData(this RibbonPanel ribbonPanel, string targetName = null)
        {
            if (string.IsNullOrWhiteSpace(targetName))
                targetName = nameof(ComboBoxData);

            targetName = RibbonSafeExtension.GenerateSafeButtonName(ribbonPanel, targetName, targetName);

            return new ComboBoxData(targetName);
        }

        public static ComboBoxMemberData SetGroupName(this ComboBoxMemberData comboBoxMemberData, string groupName)
        {
            comboBoxMemberData.GroupName = groupName;
            return comboBoxMemberData;
        }
  
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