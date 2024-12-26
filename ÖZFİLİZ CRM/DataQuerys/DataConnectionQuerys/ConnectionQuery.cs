namespace ÖZFİLİZ_CRM.DataQuerys.DataConnectionQuerys
{
    public static class ConnectionQuery
    {
        public static SqlConnection connection = new SqlConnection(DataConnection.DataCon);

        public static void OnPush()
        {
            connection.Open();
        }
    }
}
