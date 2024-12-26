namespace ÖZFİLİZ_CRM
{
    public partial class App : Application
    {
        public PageWindow windowTest { get; }
        public App(PageWindow WindowTest)
        {
            InitializeComponent();
            windowTest = WindowTest;
            SecureStorageService.GetSecureStorage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            windowTest.Page = new AppShell();
            return windowTest;
        }
    }
}