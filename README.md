# BIMPlugins.UI

[![Revit 2019-2025](https://img.shields.io/badge/Revit-2019--2025-blue.svg)](https://github.com/Samveyil/BIMPlugins.UI)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/Samveyil/BIMPlugins.UI)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Samveyil/BIMPlugins.UI/blob/master/LICENSE.txt)
[![Publish](https://github.com/Samveyil/BIMPlugins.UI/actions/workflows/publish.yml/badge.svg)](https://github.com/Samveyil/BIMPlugins.UI/actions/workflows/publish.yml)
[![NuGet](https://img.shields.io/nuget/v/BIMPlugins.UI.svg)](https://www.nuget.org/packages/BIMPlugins.UI)

Fluent extension methods to build the Revit ribbon: panels, push, pulldown, split, toggle and radio buttons,
combo boxes, text boxes and stacked items. Images are loaded from resources, files or base64,
switch automatically between light and dark themes, and texts can be localized for the Revit UI language.

## Installation

One package contains a build for each .NET runtime used by Revit. NuGet picks the right one
by the target framework of your add-in:

| Revit       | Target framework  | Built against   |
|-------------|-------------------|-----------------|
| 2019 – 2020 | `net47`           | Revit 2019 API  |
| 2021 – 2024 | `net48`           | Revit 2021 API  |
| 2025        | `net8.0-windows`  | Revit 2025 API  |

```xml
<PackageReference Include="BIMPlugins.UI" Version="1.*" />
```

The package does not ship `RevitAPI.dll`, `RevitAPIUI.dll`, `AdWindows.dll` or `UIFramework.dll`. Reference the Revit API
in your project as usual, for example through `Revit_All_Main_Versions_API_x64`.

```C#
using BIMPlugins.UI;
using BIMPlugins.UI.Utils;
```

## Panels

```C#
public class App : IExternalApplication
{
    private RibbonPanel panel;

    public Result OnStartup(UIControlledApplication application)
    {
        panel = application.CreatePanel("BIMPlugins", "Tools");   // creates the tab when it does not exist

        panel.CreatePushButton<WallsCommand>("Walls")
            .SetLargeImage("Resources/walls-light.png")
            .SetToolTip("Create walls")
            .SetLongDescription("Creates walls along the selected lines.")
            .SetContextualHelp("https://github.com/Samveyil/BIMPlugins.UI");

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        panel.Remove();
        return Result.Succeeded;
    }
}
```

`CreatePanel(panelName)` adds the panel to the Add-Ins tab. If a panel with the same name already exists,
a new panel with a unique internal name and the same title is created. `CreateOrSelectPanel` returns the existing panel instead.

Every `Set*` method returns the same item, so calls can be chained. `SetText`, `SetToolTip`, `SetLongDescription`
and `SetContextualHelp` ignore `null`, and an image that cannot be loaded leaves the current image unchanged.

## Buttons

```C#
var button  = panel.CreatePushButton<Command>();                       // text is the command class name
var guarded = panel.CreatePushButton<Command, AvailableOnDocument>("Run");

var pulldown = panel.CreatePulldownButton("Tools",
    panel.NewPushButtonData<CommandA>("A"),
    panel.NewPushButtonData<CommandB>("B"));
pulldown.CreatePushButton<CommandC>("C");

var split = panel.CreateSplitButton(
    panel.NewPushButtonData<CommandA>("A"),
    panel.NewPushButtonData<CommandB>("B"));

var toggle = panel.CreateToggleButton<ToggleCommand>("Toggle");

var radio = panel.CreateRadioButtonGroup(
    panel.NewToggleButtonData<ModeACommand>("Mode A"),
    panel.NewToggleButtonData<ModeBCommand>("Mode B"));
```

Button names are generated from the command type and made unique automatically, so the same command can be added more than once.
A command class that also implements `IExternalCommandAvailability` is used as its own availability class.

## Combo boxes and text boxes

```C#
var comboBox = panel.CreateComboBox(
        panel.NewComboBoxMemberData("Option 1"),
        panel.NewComboBoxMemberData("Option 2"))
    .SetWidth(150)
    .AddCurrentChanged((s, e) => TaskDialog.Show("ComboBox", e.NewValue.ItemText));

var textBox = panel.CreateTextBox()
    .SetPromptText("Search")
    .SetWidth(200)
    .AddEnterPressed((s, e) => TaskDialog.Show("TextBox", ((TextBox)s).Value?.ToString()));
```

## Stacked items

Create the items in the panel first, then group them:

```C#
panel.RowStackedItems(button1, button2, button3);      // up to 3 small items per stack
panel.RowLargeStackedItems(button4, button5);          // up to 2 large items per stack
panel.FlowStackedItems(button6, button7, button8);     // small image-only items in a flow panel
```

## Images

`SetImage` (16 px), `SetLargeImage` (32 px) and `SetToolTipImage` accept an `ImageSource` or a string:

- a file path, url or pack uri;
- a resource path such as `Resources/icon.png`, resolved in the add-in assembly (build action `Resource`);
- a base64 encoded image.

`SetLargeImage` on a button also sets the small image when it is not set yet. Images with several frames (`.ico`, `.tiff`)
use the frame that matches the size and the system DPI.

```C#
var image = "Resources/icon.png".GetBitmapSource();
var small = image.GetBitmapFrame(16);
```

## Light and dark themes

When an image name contains `light` or `dark`, the image for the current Revit theme is used,
for example `walls-light.png` in the light theme and `walls-dark.png` in the dark theme.
Panels created with `CreatePanel` update their images when the theme changes.

```C#
bool dark = RibbonThemeUtils.IsDark;
RibbonThemeUtils.ThemeChanged += (s, e) => { /* e.IsLight, e.IsDark */ };
```

## Localization

Set texts, tooltips, help and images for each Revit UI language. The first description is the default,
the description for the current language overrides it:

```C#
panel.UpdateRibbonDescription(settings =>
{
    settings.Add<WallsCommand>(
        new RibbonDescription { Text = "Walls", ToolTip = "Create walls" },
        new RibbonDescription(LanguageType.French) { Text = "Murs", ToolTip = "Créer des murs" });

    settings.Add("Tools",
        d => d.Text = "Tools",
        d => { d.LanguageType = LanguageType.German; d.Text = "Werkzeuge"; });
});

bool french = LanguageExtension.IsFrench;
var language = LanguageExtension.GetLanguageType();
```

Descriptions are matched by item name: `Add<TCommand>` uses the command type name, `AddDefault` applies to every item of the panel.

## Ribbon tabs and the Modify tab

```C#
panel.MoveToRibbonTab();                          // moves the panel to the Modify tab
panel.MoveToRibbonTab("Manage");                  // moves the panel to the tab with this id

var modifyPanel = RibbonModifyUtils.CreateRibbonPanel("BIMPlugins", button);
RibbonModifyUtils.RemoveRibbonPanel("BIMPlugins");

application.FindOrCreateRibbonTab("BIMPlugins");
var tab = panel.GetRibbonTab();
```

## Removing items

```C#
panel.Remove(button);       // removes one item
panel.Remove();             // hides the panel and removes it from its tab
```

## Low-level access

Get the `Autodesk.Windows` objects behind the Revit API ribbon objects:

```C#
var awItem  = button.ToAWRibbonItem();
var awPanel = panel.ToAWRibbonPanel();

button.SetShowText(false).SetItemSize(Autodesk.Windows.RibbonItemSize.Standard);
```

## Credits

Based on [ricaun.Revit.UI](https://github.com/ricaun-io/ricaun.Revit.UI).

## License

This project is [licensed](https://github.com/Samveyil/BIMPlugins.UI/blob/master/LICENSE.txt) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/Samveyil/BIMPlugins.UI/stargazers)!
