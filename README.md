# dotnetCampus.UISpy.Uno

Archive by Uno Hot Design.

Please use Uno Hot Design: <https://platform.uno/hot-design/>

![](./Docs/Images/HotDesign.png)

------

[中文文档](./README_zh-cn.md)

Recommended: ![](https://github.com/dotnet-campus/dotnetCampus.UISpy.Uno/workflows/BuildProject/badge.svg)
[![](https://img.shields.io/nuget/v/dotnetCampus.UISpy.Uno.svg)](https://www.nuget.org/packages/dotnetCampus.UISpy.Uno)

Legacy: [![](https://img.shields.io/nuget/v/UnoSpySnoopProvider.svg)](https://www.nuget.org/packages/UnoSpySnoopProvider)

dotnetCampus.UISpy.Uno is a tool for inspecting the runtime visual tree of a Skia platforms Uno app.

## Why This Tool is Needed

The reason for needing this tool is that on the Skia platform, both WPF and GTK and X11 uses a Surface to render the interface. This results in the original WPF UI debugging tools, such as SnoopWpf, only being able to see an image and not being able to obtain the correct interface structure. dotnetCampus.UISpy.Uno can effectively assist in interface development debugging on Skia-based desktop platforms, such as Skia.Wpf and Skia.Gtk and Skia.X11 and so on, enhancing the efficiency of developers' interface development, especially when debugging Skia.Gtk or Skia.X11 applications on the Linux desktop.

## Usage

In the project where the UI interface is to be debugged, follow these preparation steps:

1. Install the NuGet package named [dotnetCampus.UISpy.Uno](https://www.nuget.org/packages/dotnetCampus.UISpy.Uno).
2. Add a Grid control named SnoopRootGrid at the top level of the UI interface for subsequent display of the highlighted area. Please do not put any business logic interface in SnoopRootGrid, as the content of this SnoopRootGrid will be constantly cleared.
3. Use the `AttachDevTools` method of the `DevToolsExtensions` static type in the `dotnetCampus.UISpy.Uno` namespace, and pass `this` as a parameter to complete the preparation work. Here is an example code:

```csharp
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
#if HAS_UNO
        this.AttachDevTools();
#endif
    }
}
```

After completing the above preparation work, you can run the project, press F12 to debug the UI interface.

![](./Docs/Images/SpySampleUnoApp.png)

## Thanks

Thank you [snoopwpf](https://github.com/snoopwpf/snoopwpf), this is the inspiration for this tool.
