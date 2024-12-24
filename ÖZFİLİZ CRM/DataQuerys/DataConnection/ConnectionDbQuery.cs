using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÖZFİLİZ_CRM.DataQuerys.DataConnection
{
    public static class ConnectionDbQuery
    {
        public static string DataCon = $"Data Source={PublicClass.SetSqlServer};Persist Security Info=True;User ID={PublicClass.SetSqlUserName};Password={PublicClass.SetSqlPassword};Encrypt=True;Trust Server Certificate=True;Connection Timeout=30;Application Name=CRM;";

        public static void OnPush()
        {
            SqlConnection connection = new SqlConnection(DataCon);
            connection.Open();
        }
    }
}
