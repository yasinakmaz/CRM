using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
