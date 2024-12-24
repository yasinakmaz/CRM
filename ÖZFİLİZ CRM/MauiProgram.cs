using Microsoft.Extensions.Logging;

namespace ÖZFİLİZ_CRM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<PageWindow>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<SettingsMenu>();

            return builder.Build();
        }
    }
}
