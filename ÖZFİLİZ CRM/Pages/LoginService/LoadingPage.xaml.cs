namespace ÖZFİLİZ_CRM.Pages.LoginService;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
        UserListView.ItemsSource = UserListViewLoad.Users;
        _ = LoadUsersAsync();
    }

    private async Task LoadUsersAsync()
    {
        await UserListViewLoad.OnPushAsync();
    }

    private void BtnUserChange_Clicked(object sender, EventArgs e)
    {
		if(UserListView.IsVisible == false)
		{
			UserBorder.IsVisible = true;
			UserListView.IsVisible = true;
		}
		else
		{
			UserBorder.IsVisible = false;
			UserListView.IsVisible = false;
		}
    }
}