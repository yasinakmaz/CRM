namespace CRM.ViewModels
{
    public partial class AddServiceViewModel : ObservableObject
    {
        #region ObservableProperty
        #region BUSINESS
        [ObservableProperty]
        [Key]
        private int businessind;

        [ObservableProperty]
        private int type;

        [ObservableProperty]
        private string? businessName;

        [ObservableProperty]
        private bool businessTaxType;

        [ObservableProperty]
        private string? taxNumber;

        [ObservableProperty]
        private string? taxOffice;

        [ObservableProperty]
        private string? authNameAndSurname;

        [ObservableProperty]
        private string? phoneNumber;

        [ObservableProperty]
        private DateTime businesscreateDate;

        [ObservableProperty]
        private DateTime businesslastUpdate;

        [ObservableProperty]
        private int businesscreatorUserInd;

        [ObservableProperty]
        private int businesslastUpdateUserInd;
        #endregion

        #region EXPENSE
        [ObservableProperty]
        [Key]
        private int ind;

        [ObservableProperty]
        private int formInd;

        [ObservableProperty]
        [Column(TypeName = "money")]
        private decimal amount;

        [ObservableProperty]
        private string? comment;

        [ObservableProperty]
        private int expensecreatorUserInd;

        [ObservableProperty]
        private DateTime createdDate;

        [ObservableProperty]
        private DateTime lastUpdateDate;

        [ObservableProperty]
        private int expenselastUpdateUserInd;
        #endregion

        #region Header
        [ObservableProperty]
        [Key]
        private int headerind;

        [ObservableProperty]
        private int headertype;

        [ObservableProperty]
        private int headerbusinessInd;

        [ObservableProperty]
        private DateTime headerserviceDate;

        [ObservableProperty]
        private int headercreatorUser;

        [ObservableProperty]
        private DateTime headercreatedDate;

        [ObservableProperty]
        private int headerlastUpdateUserInd;

        [ObservableProperty]
        private DateTime headerlastUpdateDate;
        #endregion
        #endregion

        #region ObservableCollection
        public ObservableCollection<Business> Business { get; set; } = new ObservableCollection<Business>();
        public ObservableCollection<ServiceFormHeader> Header { get; set; } = new ObservableCollection<ServiceFormHeader>();
        public ObservableCollection<ServiceFormExpenseDetail> Expense { get; set; } = new ObservableCollection<ServiceFormExpenseDetail>();
        #endregion

        public AddServiceViewModel()
        {

        }

        [RelayCommand]
        public async Task AddExpense()
        {
            try
            {
                Expense.Add(new ServiceFormExpenseDetail
                {
                    AMOUNT = 1m,
                    COMMENT = string.Empty,
                });
            }
            catch (DbUpdateException ex)
            {
                var innerMsg = ex.InnerException?.Message ?? ex.Message;
                await Shell.Current.DisplayAlert("Sistem", $"Veritabanı Hatası: {innerMsg}", "Tamam");
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"SQL Hatası: {ex.Message}", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Genel Hata: {ex.Message}", "Tamam");
            }
        }


    }
}
