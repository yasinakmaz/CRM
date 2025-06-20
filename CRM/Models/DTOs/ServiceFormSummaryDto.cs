namespace CRM.Models.DTOs
{
    public class ServiceFormSummaryDto
    {
        public int IND { get; set; }
        public DateTime ServiceDate { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int ExpenseCount { get; set; }
        public string ServiceDateFormatted => ServiceDate.ToString("dd.MM.yyyy");
        public string TotalAmountFormatted => TotalAmount.ToString("C2", CultureInfo.CurrentCulture);
    }
}
