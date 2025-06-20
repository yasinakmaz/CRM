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

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                try
                {
                    var connectionString = $"Data Source={PublicSettings.MSSQLSERVER};Initial Catalog={PublicSettings.MSSQLDATABASE};Persist Security Info=False;User ID={PublicSettings.MSSQLUSERNAME};Password={PublicSettings.MSSQLPASSWORD};Encrypt=True;Trust Server Certificate=True;Connection Timeout=30;Command Timeout=300;Pooling=True;Application Name=CRM-{AppInfo.VersionString};";

                    var debugConnectionString = connectionString.Replace(PublicSettings.MSSQLPASSWORD, "***");
                    System.Diagnostics.Debug.WriteLine($"Connection String: {debugConnectionString}");

                    options.UseSqlServer(connectionString, sqloptions =>
                    {
                        sqloptions.CommandTimeout(300);
                        sqloptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                        sqloptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .EnableServiceProviderCaching(true)
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
                    .LogTo(message => System.Diagnostics.Debug.WriteLine(message))
                    .ConfigureWarnings(warnings =>
                    {
                        warnings.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                        warnings.Ignore(SqlServerEventId.DecimalTypeDefaultWarning);
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"DbContext configuration error: {ex.Message}");
                    throw;
                }
            }, ServiceLifetime.Singleton);

            builder.Services.AddTransient<AddServiceViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
