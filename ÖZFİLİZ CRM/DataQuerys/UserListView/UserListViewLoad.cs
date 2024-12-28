using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ÖZFİLİZ_CRM.DataQuerys.UserListView
{
    public class UserListViewLoad
    {
        public static ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();

        public async static Task OnPushAsync()
        {
            ConnectionQuery.OnPush();
            try
            {
                string query = "SELECT IND, UserName, Password FROM TblUser";
                using (SqlCommand cmd = new SqlCommand(query, ConnectionQuery.connection))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    Users.Clear();
                    while (await reader.ReadAsync())
                    {
                        Users.Add(new UserModel
                        {
                            IND = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2)
                        });
                    }
                    if (Users.Count == 0)
                    {
                        await Shell.Current.DisplayAlert("Sistem", "Hata: bulamadık", "Tamam");
                    }
                }
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Hata: {ex.Message}", "Tamam");
            }
        }
    }
}
