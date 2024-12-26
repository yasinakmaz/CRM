using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÖZFİLİZ_CRM.DataQuerys
{
    public class UserListViewLoad
    {
        public ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();

        private static void OnPush()
        {
            ConnectionQuery.OnPush();
            try
            {
                string query = "SELECT IND, UserName";
            }
            catch (SqlException ex)
            {

            }
            finally
            {

            }
        }
    }
}
