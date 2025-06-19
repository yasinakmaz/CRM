namespace CRM.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        private string? mssqlserver;

        [ObservableProperty]
        private string? mssqlusername;

        [ObservableProperty]
        private string? mssqlpassword;

        [ObservableProperty]
        private string? mssqldata;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string? busytext;
        #endregion

        #region Collections
        public ObservableCollection<string> databases { get; } = new ObservableCollection<string>();
        #endregion

        public SettingsViewModel()
        {
            
        }

        public async Task InitializeSettings()
        {
            await LoadSettingsAsync();
        }

        #region Load Settings Command
        private async Task LoadSettingsAsync()
        {
            try
            {
                Mssqlserver = await SecureStorage.Default.GetAsync(PublicSettings.mssqlserver) ?? "";
                Mssqlusername = await SecureStorage.Default.GetAsync(PublicSettings.mssqlusername) ?? "";
                Mssqlpassword = await SecureStorage.Default.GetAsync(PublicSettings.mssqlpassword) ?? "";
                Mssqldata = await SecureStorage.Default.GetAsync(PublicSettings.mssqldatabase) ?? "";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Ayarlar yüklenirken hata: {ex.Message}", "Tamam");
            }
        }
        #endregion

        #region Save Settings Commands
        public async Task SaveMssqlSettings()
        {
            try
            {
                IsLoading = true;
                Busytext = "Yükleniyor";
                if (!string.IsNullOrWhiteSpace(Mssqlserver))
                    await SecureStorage.Default.SetAsync(PublicSettings.mssqlserver, Mssqlserver);

                if (!string.IsNullOrWhiteSpace(Mssqlusername))
                    await SecureStorage.Default.SetAsync(PublicSettings.mssqlusername, Mssqlusername);

                if (!string.IsNullOrWhiteSpace(Mssqlpassword))
                    await SecureStorage.Default.SetAsync(PublicSettings.mssqlpassword, Mssqlpassword);

                if (!string.IsNullOrWhiteSpace(Mssqldata))
                    await SecureStorage.Default.SetAsync(PublicSettings.mssqldatabase, Mssqldata);

                await Shell.Current.DisplayAlert("Sistem", "MSSQL ayarları kaydedildi", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"MSSQL ayarları kaydedilirken hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region CheckDatabase Command
        public async Task CheckDatabase()
        {
            try
            {
                IsLoading = true;
                Busytext = "Yükleniyor";

                databases.Clear();

                if (string.IsNullOrWhiteSpace(Mssqlserver) ||
                    string.IsNullOrWhiteSpace(Mssqlusername) ||
                    string.IsNullOrWhiteSpace(Mssqlpassword))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Lütfen Server, Kullanıcı Adı ve Şifre alanlarını doldurunuz", "Tamam");
                    return;
                }

                string connectionstring = $"Server={Mssqlserver};Database=master;User Id={Mssqlusername};Password={Mssqlpassword};Connection Timeout=30;TrustServerCertificate=True;Encrypt=False;";

                using var connection = new SqlConnection(connectionstring);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name";
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    databases.Add(reader.GetString(0));
                }

                var message = databases.Count > 0
                    ? $"Bağlantı sağlandı. {databases.Count} veritabanı bulundu."
                    : "Bağlantı sağlandı ancak kullanılabilir veritabanı bulunamadı.";

                await Shell.Current.DisplayAlert("Sistem", message, "Tamam");
            }
            catch (SqlException sqlEx)
            {
                await Shell.Current.DisplayAlert("SQL Hatası", $"Veritabanı bağlantısı başarısız: {sqlEx.Message}", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Beklenmeyen hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}
