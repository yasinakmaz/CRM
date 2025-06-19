namespace CRM
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void BtnAddBusiness_Clicked(object sender, EventArgs e)
        {
            await BusinessAddPop.ShowAsync();
        }
    }
}
