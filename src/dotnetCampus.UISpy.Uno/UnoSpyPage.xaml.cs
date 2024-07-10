using Windows.UI;
using dotnetCampus.UISpy.Uno.Tree;

using Microsoft.UI.Xaml.Media.Imaging;

namespace dotnetCampus.UISpy.Uno;

public sealed partial class UnoSpyPage : Page
{
    private DependencyObject? _targetRootElement;

    public UnoSpyPage()
    {
        InitializeComponent();
        this.DebugAttachDevTools();
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
        VisualTreeElementPropertyListView.ContainerContentChanging -= VisualTreeElementPropertyListView_ContainerContentChanging;
        VisualTreeElementPropertyListView.ContainerContentChanging += VisualTreeElementPropertyListView_ContainerContentChanging;

#if HAS_UNO
        if (CalculatedInfoImage.Source is { } oldImage)
        {
            oldImage.Dispose();
        }
#endif
        var bitmap = await RenderBitmap((UIElement) node.Element);
        CalculatedInfoImage.Source = bitmap;
    }

    private void VisualTreeElementPropertyListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (args.ItemIndex % 2 != 0)
        {
            var background = new SolidColorBrush(new Color()
            {
                A = 0xFF,
                R = 0xD1,
                G = 0xD1,
                B = 0xD1,
            });
            args.ItemContainer.Background = background;
        }
        else
        {
            args.ItemContainer.Background = new SolidColorBrush(Colors.Transparent);
        }
    }

    private static async ValueTask<ImageSource> RenderBitmap(UIElement element)
    {
        var rtb = new RenderTargetBitmap();
        await rtb.RenderAsync(element);
        return rtb;
    }

    private void ReloadButton_OnClick(object sender, RoutedEventArgs args)
    {
        if (TargetRootElement is { } value)
        {
            OnTargetRootElementChanged(value);
        }
    }
}



public class ElementPropertyProxyToBackgroundConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ElementPropertyProxy elementPropertyProxy)
        {
            var color = Colors.Transparent;

            if (elementPropertyProxy.IsNotImplemented)
            {
                color = Colors.LightGray;
            }
            else if (elementPropertyProxy.IsFailed)
            {
                color = Colors.OrangeRed;
            }

            return new SolidColorBrush(color);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
