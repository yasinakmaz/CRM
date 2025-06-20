namespace CRM.Models
{
    [Table("SERVICEFORMHEADER")]
    [Index(nameof(BUSINESSIND), Name = "IX_ServiceFormHeader_BusinessInd")]
    [Index(nameof(ServiceDate), Name = "IX_ServiceFormHeader_ServiceDate")]
    [Index(nameof(BUSINESSIND), nameof(ServiceDate), Name = "IX_ServiceFormHeader_Business_Date")]
    public class ServiceFormHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        [Required]
        public int TYPE { get; set; } = 1;

        [Required]
        public int BUSINESSIND { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime ServiceDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int CreatorUser { get; set; } = 1;

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int LASTUPDATEUSERIND { get; set; } = 1;

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime LASTUPDATEDATE { get; set; } = DateTime.UtcNow;

        public virtual Business? Business { get; set; }

        public virtual ICollection<ServiceFormExpenseDetail> ExpenseDetails { get; set; } = new List<ServiceFormExpenseDetail>();

        [NotMapped]
        public decimal TotalAmount => ExpenseDetails?.Sum(e => e.AMOUNT) ?? 0;

        [NotMapped]
        public int ExpenseCount => ExpenseDetails?.Count ?? 0;

        [NotMapped]
        public string ServiceDateFormatted => ServiceDate.ToString("dd.MM.yyyy HH:mm");

        public override string ToString()
        {
            return $"Service Form {IND} - {ServiceDate:dd.MM.yyyy}";
        }

        public override bool Equals(object? obj)
        {
            return obj is ServiceFormHeader header && IND == header.IND;
        }

        public override int GetHashCode()
        {
            return IND.GetHashCode();
        }
    }
}