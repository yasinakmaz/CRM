namespace CRM.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);

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
        #endregion

        #region Save Settings Commands
        public async Task SaveMssqlSettings()
        {
            try
            {
                IsLoading = true;
                Busytext = "Ayarlar kaydediliyor...";

                if (string.IsNullOrWhiteSpace(Mssqlserver) ||
                    string.IsNullOrWhiteSpace(Mssqlusername) ||
                    string.IsNullOrWhiteSpace(Mssqlpassword))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Tüm alanları doldurunuz", "Tamam");
                    return;
                }

                var saveTasks = new List<Task>();

                if (!string.IsNullOrWhiteSpace(Mssqlserver))
                {
                    saveTasks.Add(SecureStorage.Default.SetAsync(PublicSettings.mssqlserver, Mssqlserver));
                    PublicSettings.MSSQLSERVER = Mssqlserver;
                }

                if (!string.IsNullOrWhiteSpace(Mssqlusername))
                {
                    saveTasks.Add(SecureStorage.Default.SetAsync(PublicSettings.mssqlusername, Mssqlusername));
                    PublicSettings.MSSQLUSERNAME = Mssqlusername;
                }

                if (!string.IsNullOrWhiteSpace(Mssqlpassword))
                {
                    saveTasks.Add(SecureStorage.Default.SetAsync(PublicSettings.mssqlpassword, Mssqlpassword));
                    PublicSettings.MSSQLPASSWORD = Mssqlpassword;
                }

                if (!string.IsNullOrWhiteSpace(Mssqldata))
                {
                    saveTasks.Add(SecureStorage.Default.SetAsync(PublicSettings.mssqldatabase, Mssqldata));
                    PublicSettings.MSSQLDATABASE = Mssqldata;
                }

                await Task.WhenAll(saveTasks);

                await Shell.Current.DisplayAlert("Sistem", "MSSQL ayarları başarıyla kaydedildi", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"MSSQL ayarları kaydedilirken hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsLoading = false;
                Busytext = string.Empty;
            }
        }
        #endregion

        #region CheckDatabase Command
        public async Task CheckDatabase()
        {
            if (IsLoading) return;

            await _connectionSemaphore.WaitAsync();
            try
            {
                IsLoading = true;
                Busytext = "Veritabanı bağlantısı kontrol ediliyor...";

                databases.Clear();

                if (string.IsNullOrWhiteSpace(Mssqlserver) ||
                    string.IsNullOrWhiteSpace(Mssqlusername) ||
                    string.IsNullOrWhiteSpace(Mssqlpassword))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Lütfen Server, Kullanıcı Adı ve Şifre alanlarını doldurunuz", "Tamam");
                    return;
                }

                var connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = Mssqlserver,
                    InitialCatalog = "master",
                    UserID = Mssqlusername,
                    Password = Mssqlpassword,
                    ConnectTimeout = 15,
                    CommandTimeout = 30,
                    TrustServerCertificate = true,
                    Encrypt = false,
                    Pooling = true,
                    MinPoolSize = 1,
                    MaxPoolSize = 10
                };

                using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                await connection.OpenAsync(cts.Token);

                const string query = @"
                    SELECT name 
                    FROM sys.databases 
                    WHERE database_id > 4 -- Exclude system databases more efficiently
                        AND state = 0 -- Only online databases
                        AND is_read_only = 0 -- Only writable databases
                    ORDER BY name";

                using var command = new SqlCommand(query, connection);
                command.CommandTimeout = 15;

                using var reader = await command.ExecuteReaderAsync(cts.Token);

                var databaseList = new List<string>();
                while (await reader.ReadAsync(cts.Token))
                {
                    databaseList.Add(reader.GetString(0));
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var db in databaseList)
                    {
                        databases.Add(db);
                    }
                });

                var message = databases.Count > 0
                    ? $"Bağlantı başarılı! {databases.Count} veritabanı bulundu."
                    : "Bağlantı başarılı ancak kullanılabilir veritabanı bulunamadı.";

                await Shell.Current.DisplayAlert("Sistem", message, "Tamam");

                if (databases.Count > 0)
                {
                    Busytext = "Bağlantı kalitesi test ediliyor...";
                    await TestConnectionQuality(connectionStringBuilder.ConnectionString, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                await Shell.Current.DisplayAlert("Zaman Aşımı", "Veritabanı bağlantısı zaman aşımına uğradı. Sunucu ayarlarınızı kontrol ediniz.", "Tamam");
            }
            catch (SqlException sqlEx)
            {
                string errorMessage = sqlEx.Number switch
                {
                    2 => "Sunucu bulunamadı. Server adresini kontrol ediniz.",
                    18456 => "Kullanıcı adı veya şifre hatalı.",
                    53 => "Ağ bağlantısı hatası. Sunucu erişilebilir durumda değil.",
                    _ => $"SQL Hatası: {sqlEx.Message}"
                };

                await Shell.Current.DisplayAlert("Bağlantı Hatası", errorMessage, "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Beklenmeyen hata: {ex.Message}", "Tamam");
            }
            finally
            {
                IsLoading = false;
                Busytext = string.Empty;
                _connectionSemaphore.Release();
            }
        }

        private async Task TestConnectionQuality(string connectionString, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                using var command = new SqlCommand("SELECT 1", connection);
                await command.ExecuteScalarAsync(cancellationToken);

                stopwatch.Stop();

                var responseTime = stopwatch.ElapsedMilliseconds;
                string qualityMessage = responseTime switch
                {
                    < 50 => "Mükemmel",
                    < 100 => "İyi",
                    < 300 => "Orta",
                    _ => "Yavaş"
                };

                System.Diagnostics.Debug.WriteLine($"Bağlantı kalitesi: {qualityMessage} ({responseTime}ms)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Bağlantı kalite testi hatası: {ex.Message}");
            }
        }
        #endregion

        public void Dispose()
        {
            _connectionSemaphore?.Dispose();
        }
    }
}