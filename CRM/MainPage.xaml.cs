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

        private async void OnBusinessSaved(object sender, EventArgs e)
        {
            BusinessAddPop.Dismiss();

            if (BindingContext is MainPageViewModel vm)
            {
                await vm.AddServiceVm.LoadBusinessListCommand.ExecuteAsync(null);
            }
        }
    }
}
