using dotnetCampus.UISpy.Uno;

namespace dotnetCampus.SampleUnoApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();

        this.AttachDevTools();
    }

    public dotnetCampus.SampleUnoApp.Localizations.Localizations.ILocalized_Root Lang => dotnetCampus.SampleUnoApp.Localizations.Lang.Current;

    public void Button_OnClick(object sender, RoutedEventArgs args)
    {
        this.ShowUnoSpyWindow();
    }
}
