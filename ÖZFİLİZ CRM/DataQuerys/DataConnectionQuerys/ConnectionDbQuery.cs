namespace ÖZFİLİZ_CRM.DataQuerys.DataConnectionQuerys
{
    public static class ConnectionDbQuery
    {
        public static string DataCon = $"Data Source={PublicClass.SetSqlServer};Persist Security Info=True;User ID={PublicClass.SetSqlUserName};Password={PublicClass.SetSqlPassword};Encrypt=True;Trust Server Certificate=True;Connection Timeout=30;Application Name=CRM;";
        public static SqlConnection connection = new SqlConnection(DataCon);

        public static void OnPush()
        {
            connection.Open();
        }
    }
}
