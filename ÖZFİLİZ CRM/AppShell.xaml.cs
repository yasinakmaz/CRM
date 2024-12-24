namespace ÖZFİLİZ_CRM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PageWindow), typeof(PageWindow));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(SettingsMenu), typeof(SettingsMenu));
        }
    }
}
