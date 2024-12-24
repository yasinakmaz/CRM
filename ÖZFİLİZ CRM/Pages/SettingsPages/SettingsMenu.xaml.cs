using ÖZFİLİZ_CRM.DataQuerys.DataConnection;

namespace ÖZFİLİZ_CRM.Pages.SettingsPages;

public partial class SettingsMenu : ContentPage
{
	public SettingsMenu()
	{
		InitializeComponent();
	}

    private async void DbCon_Clicked(object sender, EventArgs e)
    {
        try
        {
            ConnectionDbQuery.OnPush();
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
        TxtSqlServer.Text = PublicClass.SetSqlServer;
        SecureStorageService.SetSqlServer();
    }

    private void TxtSqlUserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        TxtSqlUserName.Text = PublicClass.SetSqlServer;
        SecureStorageService.SetSqlUserName();
    }

    private void TxtSqlPassword_TextChanged(object sender, TextChangedEventArgs e)
    {
        TxtSqlPassword.Text = PublicClass.SetSqlServer;
        SecureStorageService.SetSqlPassword();
    }
}