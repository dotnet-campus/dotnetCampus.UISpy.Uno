using dotnetCampus.SampleUnoApp.Localizations.Localizations;
using dotnetCampus.UISpy.Uno;

namespace dotnetCampus.SampleUnoApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();

        this.AttachDevTools();
    }

}
