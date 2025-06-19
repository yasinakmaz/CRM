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

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await SettingsVm.InitializeSettings();
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
        }
    }
}
