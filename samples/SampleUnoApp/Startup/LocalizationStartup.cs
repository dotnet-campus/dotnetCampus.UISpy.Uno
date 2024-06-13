using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dotnetCampus.SampleUnoApp.Localizations;

namespace dotnetCampus.SampleUnoApp.Startup;

public static class LocalizationStartup
{
    public static Application UseLocalization(this Application app)
    {
        Lang.SetCurrent(CultureInfo.CurrentUICulture.Name switch
        {
            "zh-Hans" or "zh-Hant" or "zh-CN" or "zh-HK" or "zh-MO" or "zh-TW" or "zh-SG" => "zh-hans",
            _ => "en"
        });
        return app;
    }
}
