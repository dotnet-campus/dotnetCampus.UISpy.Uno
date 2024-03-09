namespace SpySnoopDemo;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
#if HAS_UNO
        UnoSpySnoop.SpySnoop.StartSpyUI(SnoopRootGrid);
#endif
    }
}
