namespace dotnetCampus.SampleUnoApp.Startup;

public static class Startup
{
    public static Application SetupApp(this Application app)
    {
        return app
                .UseLocalization()
            ;
    }

    public static Application AfterLaunched(this Application app)
    {
        return app
                .EmptyDemo()
            ;
    }

    public static Application AfterMainWindowShown(this Application app)
    {
        return app
                .EmptyDemo()
            ;
    }

    public static Application EmptyDemo(this Application app)
    {
        return app;
    }
}
