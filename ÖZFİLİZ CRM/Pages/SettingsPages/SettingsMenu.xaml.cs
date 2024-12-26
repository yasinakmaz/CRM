using ÖZFİLİZ_CRM.DataQuerys.DataConnection;

namespace ÖZFİLİZ_CRM.Pages.SettingsPages;
public partial class SettingsMenu : ContentPage
{
	public SettingsMenu()
	{
		InitializeComponent();
        GetLoad();
	}

    private void GetLoad()
    {
        TxtSqlServer.Text = PublicClass.SetSqlServer;
        TxtSqlUserName.Text = PublicClass.SetSqlUserName;
        TxtSqlPassword.Text = PublicClass.SetSqlPassword;

        if (!string.IsNullOrEmpty(PublicClass.SetSqlDbName))
        {
            DataPicker.Items.Add(PublicClass.SetSqlDbName);
            DataPicker.SelectedIndex = 0;
        }
    }

    private async void DbCon_Clicked(object sender, EventArgs e)
    {
        try
        {
            List<string> databases = LoadDbQuery.OnPush();
            DataPicker.ItemsSource = databases;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veritabanları yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
        }
    }

    private void TxtSqlServer_TextChanged(object sender, TextChangedEventArgs e)
    {
        PublicClass.SetSqlServer = e.NewTextValue;
    }

    private void TxtSqlUserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        PublicClass.SetSqlUserName = e.NewTextValue;
    }

    private void TxtSqlPassword_TextChanged(object sender, TextChangedEventArgs e)
    {
        PublicClass.SetSqlPassword = e.NewTextValue;
    }

    private async void BtnSqlSave_Clicked(object sender, EventArgs e)
    {
        await SecureStorageService.SetSqlServer();
        await SecureStorageService.SetSqlUserName();
        await SecureStorageService.SetSqlPassword();
        await SecureStorageService.SetSqlDbName();
    }

    private void DataPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DataPicker.SelectedItem != null)
        {
            string selectedItem = DataPicker.SelectedItem.ToString();
            PublicClass.SetSqlDbName = selectedItem;
        }
    }

    private async void BtnBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LoadingPage)}");
    }
}