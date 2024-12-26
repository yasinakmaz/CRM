using Microsoft.UI.Xaml.Automation.Peers;

namespace ÖZFİLİZ_CRM.Pages.LoginService;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
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