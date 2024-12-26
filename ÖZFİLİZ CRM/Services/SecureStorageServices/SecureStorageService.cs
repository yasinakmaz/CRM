using Microsoft.Maui.Storage;

namespace ÖZFİLİZ_CRM.Services.SecureStorageServices
{
    public static class SecureStorageService
    {
        public static async Task CreateSecureStorage()
        {
            await SecureStorage.Default.SetAsync(PublicClass.Baslangic, "0");
            await SecureStorage.Default.SetAsync(PublicClass.SqlServer, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlUserName, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlPassword, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlDbName, "");
        }

        public static async Task SetSecureStorageDefault()
        {
            await SecureStorage.Default.SetAsync(PublicClass.Baslangic, "0");
            await SecureStorage.Default.SetAsync(PublicClass.SqlServer, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlUserName, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlPassword, "");
            await SecureStorage.Default.SetAsync(PublicClass.SqlDbName, "");
        }
        public static async Task GetSecureStorage()
        {
           PublicClass.SetBaslangic = await SecureStorage.Default.GetAsync(PublicClass.Baslangic) ?? "0";
           PublicClass.SetSqlServer = await SecureStorage.Default.GetAsync(PublicClass.SqlServer) ?? "127.0.0.1";
           PublicClass.SetSqlUserName = await SecureStorage.Default.GetAsync(PublicClass.SqlUserName) ?? "sa";
           PublicClass.SetSqlPassword = await SecureStorage.Default.GetAsync(PublicClass.SqlPassword) ?? "";
           PublicClass.SetSqlDbName = await SecureStorage.Default.GetAsync(PublicClass.SqlDbName) ?? "";
        }

        public static async Task SetBaslangicAsync()
        {
           await SecureStorage.Default.SetAsync(PublicClass.Baslangic, $"{PublicClass.SetBaslangic}");
        }
        public static async Task SetSqlServer()
        {
            await SecureStorage.Default.SetAsync(PublicClass.SqlServer, $"{PublicClass.SetSqlServer}");
        }
        public static async Task SetSqlUserName()
        {
            await SecureStorage.Default.SetAsync(PublicClass.SqlUserName, $"{PublicClass.SetSqlUserName}");
        }
        public static async Task SetSqlPassword()
        {
            await SecureStorage.Default.SetAsync(PublicClass.SqlPassword, $"{PublicClass.SetSqlPassword}");
        }
        public static async Task SetSqlDbName()
        {
            await SecureStorage.Default.SetAsync(PublicClass.SqlDbName, $"{PublicClass.SetSqlDbName}");
        }
    }
}
