namespace ÖZFİLİZ_CRM.Pages.WindowPage;

public partial class PageWindow : Window
{
	public PageWindow()
	{
		InitializeComponent();
	}

    private async void BtnSettings_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(SettingsMenu)}");
    }
}