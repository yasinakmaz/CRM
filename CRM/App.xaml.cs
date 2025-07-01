namespace CRM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected async override void OnStart()
        {
            base.OnStart();
            await LoadSettings();
        }

        private async Task LoadSettings()
        {
            try
            {
                string Mssqlserver, Mssqlusername, Mssqlpassword, Mssqldata;
                var tasks = new[]
                {
                    SecureStorage.Default.GetAsync(PublicSettings.mssqlserver),
                    SecureStorage.Default.GetAsync(PublicSettings.mssqlusername),
                    SecureStorage.Default.GetAsync(PublicSettings.mssqlpassword),
                    SecureStorage.Default.GetAsync(PublicSettings.mssqldatabase)
                };

                var results = await Task.WhenAll(tasks);

                Mssqlserver = results[0] ?? string.Empty;
                Mssqlusername = results[1] ?? string.Empty;
                Mssqlpassword = results[2] ?? string.Empty;
                Mssqldata = results[3] ?? string.Empty;

                PublicSettings.MSSQLSERVER = Mssqlserver;
                PublicSettings.MSSQLUSERNAME = Mssqlusername;
                PublicSettings.MSSQLPASSWORD = Mssqlpassword;
                PublicSettings.MSSQLDATABASE = Mssqldata;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Ayarlar yüklenirken hata: {ex.Message}", "Tamam");
            }
        }
    }
}