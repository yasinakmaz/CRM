namespace CRM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .ConfigureSyncfusionCore()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("EthosNova-Bold.ttf", "EthosNovaBold");
                    fonts.AddFont("EthosNova-Medium.ttf", "EthosNovaMedium");
                    fonts.AddFont("EthosNova-Heavy.ttf", "EthosNovaHeavy");
                    fonts.AddFont("EthosNova-Regular.ttf", "EthosNovaRegular");
                });

            builder.Services.AddMemoryCache();

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                var connectionString = "Data Source=.;Initial Catalog=CRM;User ID=sa;Password=123456a.A;TrustServerCertificate=True;Encrypt=False;";
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(300);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableServiceProviderCaching(false);

#if DEBUG
                options.EnableDetailedErrors(true)
                       .EnableSensitiveDataLogging(true)
                       .LogTo(message => System.Diagnostics.Debug.WriteLine($"EF Core: {message}"), LogLevel.Warning);
#endif
            });

            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddTransient<AddServiceViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);
#endif

            return builder.Build();
        }
    }
}