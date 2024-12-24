using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÖZFİLİZ_CRM.DataCon
{
    public static class DataConnection
    {
        public static string DataCon = $"Data Source={PublicClass.SetSqlServer};Initial Catalog={PublicClass.SetSqlDbName};Persist Security Info=True;User ID={PublicClass.SetSqlUserName};Password={PublicClass.SetSqlPassword};Encrypt=True;Trust Server Certificate=True;Connection Timeout=30;Application Name=CRM;";
    }
}
