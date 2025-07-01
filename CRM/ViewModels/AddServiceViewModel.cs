namespace CRM.ViewModels
{
    public partial class AddServiceViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly SemaphoreSlim _saveSemaphore = new(1, 1);

        #region ObservableProperty
        #region BUSINESS
        [ObservableProperty]
        private int businessind;

        [ObservableProperty]
        private string type = string.Empty;

        [ObservableProperty]
        private string? businessName;

        [ObservableProperty]
        private string businessTaxType = "Gerçek";

        [ObservableProperty]
        private string? taxNumber;

        [ObservableProperty]
        private string? taxOffice;

        [ObservableProperty]
        private string? authNameAndSurname;

        [ObservableProperty]
        private string? phoneNumber;

        [ObservableProperty]
        private DateTime businesscreateDate = DateTime.UtcNow;

        [ObservableProperty]
        private DateTime businesslastUpdate;

        [ObservableProperty]
        private int businesscreatorUserInd;

        [ObservableProperty]
        private int businesslastUpdateUserInd;

        [ObservableProperty]
        private Business? selectedBusiness;
        #endregion

        #region EXPENSE
        [ObservableProperty]
        private int ind;

        [ObservableProperty]
        private int formInd;

        [ObservableProperty]
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
        private int headerind;

        [ObservableProperty]
        private int headertype;

        [ObservableProperty]
        private int headerbusinessInd;

        [ObservableProperty]
        private DateTime headerserviceDate = DateTime.Now;

        [ObservableProperty]
        private int headercreatorUser;

        [ObservableProperty]
        private DateTime headercreatedDate;

        [ObservableProperty]
        private int headerlastUpdateUserInd;

        [ObservableProperty]
        private DateTime headerlastUpdateDate;
        #endregion

        [ObservableProperty]
        private bool isLoading;
        #endregion

        #region ObservableCollection
        public ObservableCollection<Business> Business { get; set; } = new ObservableCollection<Business>();
        public ObservableCollection<ServiceFormHeader> Header { get; set; } = new ObservableCollection<ServiceFormHeader>();
        public ObservableCollection<ServiceFormExpenseDetail> Expense { get; set; } = new ObservableCollection<ServiceFormExpenseDetail>();
        #endregion

        public AddServiceViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            _ = Task.Run(async () =>
            {
                await LoadBusinessList();
                await EnsureDatabaseExists();
            });
        }

        private async Task EnsureDatabaseExists()
        {
            try
            {
                await _databaseService.EnsureDatabaseCreatedAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Veritabanı Hatası",
                    $"Veritabanı oluşturulamadı: {ex.Message}", "Tamam");
            }
        }

        partial void OnSelectedBusinessChanged(Business? value)
        {
            if (value != null)
            {
                Businessind = value.IND;
                Type = value.TYPE;
                BusinessName = value.BUSINESSNAME;
                BusinessTaxType = value.TypeText;
                TaxNumber = value.TAXNUMBER;
                TaxOffice = value.TAXOFFICE;
                AuthNameAndSurname = value.AUTHNAMEANDSURNAME;
                PhoneNumber = value.PHONENUMBER;
            }
        }

        [RelayCommand]
        public async Task AddBusiness()
        {
            if (IsLoading) return;

            await _saveSemaphore.WaitAsync();
            try
            {
                IsLoading = true;

                if (string.IsNullOrWhiteSpace(BusinessName))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Firma adı gereklidir", "Tamam");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Type))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Firma türü gereklidir", "Tamam");
                    return;
                }

                using var context = _databaseService.CreateContext();

                await context.Database.EnsureCreatedAsync();

                var existingBusiness = await context.BUSINESS
                    .AsNoTracking()
                    .Where(b => b.BUSINESSNAME == BusinessName)
                    .FirstOrDefaultAsync();

                if (existingBusiness != null)
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Bu firma adı zaten kayıtlı", "Tamam");
                    return;
                }

                var businessitem = new Business
                {
                    TYPE = Type,
                    BUSINESSNAME = BusinessName,
                    BUSINESSTAXTYPE = BusinessTaxType == "Tüzel",
                    TAXNUMBER = TaxNumber,
                    TAXOFFICE = TaxOffice,
                    AUTHNAMEANDSURNAME = AuthNameAndSurname,
                    PHONENUMBER = PhoneNumber,
                    CREATEDATE = DateTime.Now,
                    LASTUPDATE = DateTime.Now,
                    CREATORUSERIND = 1,
                    LASTUPDATEUSERIND = 1
                };

                context.BUSINESS.Add(businessitem);
                int result = await context.SaveChangesAsync();

                if (result > 0)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Business.Insert(0, businessitem);
                    });

                    ClearBusinessForm();

                    await Shell.Current.DisplayAlert("Sistem", "Başarıyla Kaydedildi", "Tamam");
                }
            }
            catch (DbUpdateException ex)
            {
                var innerMsg = ex.InnerException?.Message ?? ex.Message;
                await HandleDatabaseError("Veritabanı Hatası", innerMsg);
            }
            catch (SqlException ex)
            {
                await HandleDatabaseError("SQL Hatası", ex.Message);
            }
            catch (Exception ex)
            {
                await HandleDatabaseError("Genel Hata", ex.Message);
            }
            finally
            {
                IsLoading = false;
                _saveSemaphore.Release();
            }
        }

        [RelayCommand]
        public async Task AddExpense()
        {
            try
            {
                var newExpense = new ServiceFormExpenseDetail
                {
                    AMOUNT = 0m,
                    COMMENT = string.Empty,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDATE = DateTime.Now,
                    CREATORUSERIND = 1,
                    LASTUPDATEUSERIND = 1
                };

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Expense.Add(newExpense);
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Masraf eklenirken hata: {ex.Message}", "Tamam");
            }
        }

        [RelayCommand]
        public async Task LoadBusinessList()
        {
            try
            {
                if (!await _databaseService.TestConnectionAsync())
                {
                    await Shell.Current.DisplayAlert("Sistem","Database connection failed, skipping business list load", "Tamam");
                    return;
                }

                using var context = _databaseService.CreateContext();

                await context.Database.EnsureCreatedAsync();

                var businesses = await context.BUSINESS
                    .AsNoTracking()
                    .OrderByDescending(b => b.CREATEDATE)
                    .ToListAsync();

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Business.Clear();
                    foreach (var business in businesses)
                    {
                        Business.Add(business);
                    }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Business list load error: {ex.Message}", "Tamam");
            }
        }

        [RelayCommand]
        public async Task SaveServiceForm()
        {
            if (IsLoading) return;

            await _saveSemaphore.WaitAsync();
            try
            {
                IsLoading = true;

                if (Businessind <= 0)
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Lütfen müşteri seçiniz", "Tamam");
                    return;
                }

                if (!Expense.Any() || Expense.All(e => e.AMOUNT <= 0))
                {
                    await Shell.Current.DisplayAlert("Uyarı", "En az bir masraf kalemi ekleyiniz", "Tamam");
                    return;
                }

                using var context = _databaseService.CreateContext();
                using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    var header = new ServiceFormHeader
                    {
                        TYPE = 1,
                        BUSINESSIND = Businessind,
                        ServiceDate = HeaderserviceDate,
                        CreatorUser = 1,
                        CreatedDate = DateTime.Now,
                        LASTUPDATEUSERIND = 1,
                        LASTUPDATEDATE = DateTime.Now
                    };

                    context.SERVICEFORMHEADER.Add(header);
                    await context.SaveChangesAsync();

                    int headerInd = header.IND;

                    var expenseDetails = Expense
                        .Where(e => e.AMOUNT > 0)
                        .Select(e => new ServiceFormExpenseDetail
                        {
                            FORMIND = headerInd,
                            AMOUNT = e.AMOUNT,
                            COMMENT = e.COMMENT ?? string.Empty,
                            CREATORUSERIND = 1,
                            CREATEDDATE = DateTime.Now,
                            LASTUPDATEDATE = DateTime.Now,
                            LASTUPDATEUSERIND = 1
                        }).ToList();

                    if (expenseDetails.Any())
                    {
                        context.SERVICEFORMEXPENSEDETAIL.AddRange(expenseDetails);
                        await context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    ClearServiceForm();

                    await Shell.Current.DisplayAlert("Sistem", "Servis formu başarıyla kaydedildi", "Tamam");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                await HandleDatabaseError("Servis Formu Kayıt Hatası", ex.Message);
            }
            finally
            {
                IsLoading = false;
                _saveSemaphore.Release();
            }
        }

        private async Task HandleDatabaseError(string title, string message)
        {
            await Shell.Current.DisplayAlert(title, message, "Tamam");

            bool copyToClipboard = await Shell.Current.DisplayAlert("Sistem", "Hata mesajı panoya kopyalansın mı?", "Evet", "Hayır");
            if (copyToClipboard)
            {
                await Clipboard.SetTextAsync(message);
            }
        }

        private void ClearBusinessForm()
        {
            Type = string.Empty;
            BusinessName = string.Empty;
            BusinessTaxType = "Gerçek";
            TaxNumber = string.Empty;
            TaxOffice = string.Empty;
            AuthNameAndSurname = string.Empty;
            PhoneNumber = string.Empty;
        }

        private void ClearServiceForm()
        {
            Businessind = 0;
            SelectedBusiness = null;
            HeaderserviceDate = DateTime.Now;
            Type = string.Empty;
            BusinessName = string.Empty;
            BusinessTaxType = "Gerçek";
            TaxNumber = string.Empty;
            TaxOffice = string.Empty;
            AuthNameAndSurname = string.Empty;
            PhoneNumber = string.Empty;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Expense.Clear();
            });
        }

        public void Dispose()
        {
            _saveSemaphore?.Dispose();
        }
    }
}