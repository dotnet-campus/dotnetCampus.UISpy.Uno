namespace dotnetCampus.UISpy.Uno;

internal class UnoSpyWindow : Window
{
    public UnoSpyWindow(DependencyObject rootElement)
    {
        Content = new UnoSpyPage
        {
            TargetRootElement = rootElement,
        };
#if DEBUG
        this.EnableHotReload();
#endif
    }
}
