using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÖZFİLİZ_CRM.DataQuerys.LoadDb
{
    public static class LoadDbQuery
    {
        public static List<string> OnPush()
        {
            ConnectionDbQuery.OnPush();
            var items = new List<string>();
                string query = "SELECT name FROM sys.databases;";
                using (SqlCommand command = new SqlCommand(query, ConnectionDbQuery.connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(reader.GetString(0));
                    }
                }
            return items;
        }
    }
}
