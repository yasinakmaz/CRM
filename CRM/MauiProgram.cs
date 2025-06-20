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
                var connectionString = BuildConnectionString();

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(30);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 2,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableServiceProviderCaching(true);

#if DEBUG
                options.EnableDetailedErrors(true)
                       .EnableSensitiveDataLogging(true)
                       .LogTo(message => System.Diagnostics.Debug.WriteLine(message), LogLevel.Warning);
#endif
            });

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

        private static string BuildConnectionString()
        {
            try
            {
                return $"Data Source={PublicSettings.MSSQLSERVER};" +
                       $"Initial Catalog={PublicSettings.MSSQLDATABASE};" +
                       $"User ID={PublicSettings.MSSQLUSERNAME};" +
                       $"Password={PublicSettings.MSSQLPASSWORD};" +
                       $"TrustServerCertificate=True;" +
                       $"Encrypt=True;" +
                       $"Connection Timeout=15;" +
                       $"Command Timeout=30;" +
                       $"Pooling=True;" +
                       $"Application Name=CRM-{AppInfo.VersionString};";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Connection string build error: {ex.Message}");
                return "Data Source=.;Initial Catalog=CRM;User ID=sa;Password=123456a.A;TrustServerCertificate=True;Encrypt=True;";
            }
        }
    }
}