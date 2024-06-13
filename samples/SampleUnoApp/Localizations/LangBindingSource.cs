using dotnetCampus.SampleUnoApp.Localizations.Localizations;

namespace dotnetCampus.SampleUnoApp.Localizations;

public class LangBindingSource
{
    public ILocalized_Root Lang => dotnetCampus.SampleUnoApp.Localizations.Lang.Current;
}
