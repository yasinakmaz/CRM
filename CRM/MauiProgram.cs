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
                    sqlOptions.CommandTimeout(300);
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableServiceProviderCaching(true);

#if DEBUG
                options.EnableDetailedErrors(true)
                       .EnableSensitiveDataLogging(true)
                       .LogTo(message => Shell.Current.DisplayAlert("Sql Hatası",message,"Tamam"), LogLevel.Warning);
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
                       $"Connection Timeout=30;" + 
                       $"Command Timeout=300;" +
                       $"Pooling=True;" +
                       $"Min Pool Size=0;" +
                       $"Max Pool Size=50;" +
                       $"Application Name=CRM-{AppInfo.VersionString};";
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Sistem",$"Connection string build error: {ex.Message}","Tamam");
                return "Data Source=.;Initial Catalog=CRM;User ID=sa;Password=123456a.A;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
            }
        }
    }
}