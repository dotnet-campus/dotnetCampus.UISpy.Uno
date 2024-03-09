# UnoSpySnoop

UnoSpySnoop 是一款用来辅助调试 Skia 平台下的 UNO 应用 UI 界面的工具

## 为什么需要此工具

因为在 Skia 平台下，无论是 WPF 还是 GTK 都采用一个 Surface 来渲染界面。这就导致了原本的 WPF 的 UI 调试工具，如 SnoopWpf 等工具，将只能看到一张图片而不能获取正确的界面结构。通过 UnoSpySnoop 可以很好的在基于 Skia 的桌面平台，如 Skia.Wpf 和 Skia.Gtk 上，进行辅助界面开发调试，提高开发者的界面开发效率，特别是调试在 Linux 桌面上的 Skia.Gtk 应用的时候

## 使用方法

在准备被调试 UI 界面的项目里面，进行如下的准备工作步骤

1. 安装名为 UnoSpySnoopProvider 的 NuGet 包
1. 在最顶层的 UI 界面上添加一个名为 SnoopRootGrid 的 Grid 控件，用于后续显示高亮区域。请不要在 SnoopRootGrid 里面放入任何业务逻辑界面，因为此 SnoopRootGrid 的内容将会被不断清空
1. 使用 UnoSpySnoop 命名空间下的 SpySnoop 静态类型的 StartSpyUI 方法，传入 SnoopRootGrid 作为参数，即可完成准备工作，示例代码如下

```csharp
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        UnoSpySnoop.SpySnoop.StartSpyUI(SnoopRootGrid);
    }
}
```

完成以上准备工作之后，即可运行项目，随后打开 UnoSpySnoopDebugger 工具，选择运行的项目，然后点击 `Start UI Spy` 按钮，即可进行调试 UI 的界面

![](./Docs/Images/SelectDebugProcess.png)

## 感谢

感谢 https://github.com/snoopwpf/snoopwpf 工具提供的灵感来源以及 UI 界面布局