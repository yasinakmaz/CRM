namespace CRM.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        public AddServiceViewModel AddServiceVm { get; }
        public SettingsViewModel SettingsVm { get; }

        public MainPageViewModel(AddServiceViewModel addservicevm, SettingsViewModel settingsvm)
        {
            AddServiceVm = addservicevm;
            SettingsVm = settingsvm;

            _ = Task.Run(LoadDataAsync);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await SettingsVm.InitializeSettings();

                await AddServiceVm.LoadBusinessListCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"LoadDataAsync error: {ex.Message}", "Tamam");
            }
        }

        [RelayCommand]
        private async Task CheckDatabase()
        {
            await SettingsVm.CheckDatabase();
        }

        [RelayCommand]
        private async Task SaveMssqlSettings()
        {
            await SettingsVm.SaveMssqlSettings();

            await AddServiceVm.LoadBusinessListCommand.ExecuteAsync(null);
        }
    }
}