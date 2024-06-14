using dotnetCampus.UISpy.Uno.Tree;

using Microsoft.UI.Xaml.Media.Imaging;

namespace dotnetCampus.UISpy.Uno;

public sealed partial class UnoSpyPage : Page
{
    private DependencyObject? _targetRootElement;

    public UnoSpyPage()
    {
        InitializeComponent();
    }

    public DependencyObject? TargetRootElement
    {
        get => _targetRootElement;
        set
        {
            if (Equals(_targetRootElement, value))
            {
                return;
            }

            _targetRootElement = value;

            if (value is not null)
            {
                OnTargetRootElementChanged(value);
            }
        }
    }

    private void OnTargetRootElementChanged(DependencyObject rootElement)
    {
        var tree = ElementProxyTree.BuildVisualTree(rootElement);
        VisualTreeView.ItemsSource = new[] { tree.Root };
    }

    private async void VisualTreeView_SelectionChanged(TreeView sender, TreeViewSelectionChangedEventArgs args)
    {
        if (args.AddedItems.FirstOrDefault() is not ElementProxy node)
        {
            return;
        }

        var propertyList = node.GetProperties();
        VisualTreeElementPropertyListView.ItemsSource = propertyList;

#if HAS_UNO
        if (CalculatedInfoImage.Source is { } oldImage)
        {
            oldImage.Dispose();
        }
#endif
        var bitmap = await RenderBitmap((UIElement)node.Element);
        CalculatedInfoImage.Source = bitmap;
    }

    private static async ValueTask<ImageSource> RenderBitmap(UIElement element)
    {
        var rtb = new RenderTargetBitmap();
        await rtb.RenderAsync(element);
        return rtb;
    }
}
