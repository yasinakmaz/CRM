namespace CRM.Services
{
    public class DatabaseService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public DatabaseService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public AppDbContext CreateContext()
        {
            var connectionString = BuildConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(sqlOptions =>
            {
                sqlOptions.CommandTimeout(300);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableServiceProviderCaching(false);

#if DEBUG
            optionsBuilder.EnableDetailedErrors(true)
                          .EnableSensitiveDataLogging(true);
#endif

            return new AppDbContext(optionsBuilder.Options);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var context = CreateContext();
                await context.Database.CanConnectAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem",$"Connection test failed: {ex.Message}","Tamam");
                return false;
            }
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            try
            {
                using var context = CreateContext();
                await context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem",$"Database creation failed: {ex.Message}","Tamam");
                throw;
            }
        }

        private static async Task<string> BuildConnectionString()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PublicSettings.MSSQLSERVER) ||
                    string.IsNullOrWhiteSpace(PublicSettings.MSSQLUSERNAME) ||
                    string.IsNullOrWhiteSpace(PublicSettings.MSSQLPASSWORD))
                {
                    return "Data Source=.;Initial Catalog=CRM;User ID=sa;Password=123456a.A;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;Command Timeout=300;";
                }

                var connectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = PublicSettings.MSSQLSERVER,
                    InitialCatalog = PublicSettings.MSSQLDATABASE ?? "CRM",
                    UserID = PublicSettings.MSSQLUSERNAME,
                    Password = PublicSettings.MSSQLPASSWORD,
                    TrustServerCertificate = true,
                    Encrypt = false,
                    ConnectTimeout = 30,
                    CommandTimeout = 300,
                    Pooling = true,
                    MinPoolSize = 0,
                    MaxPoolSize = 50,
                    ApplicationName = $"CRM-{AppInfo.VersionString}"
                };

                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem",$"Connection string build error: {ex.Message}","Tamam");
                return "Data Source=.;Initial Catalog=CRM;User ID=sa;Password=123456a.A;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;";
            }
        }
    }
}
