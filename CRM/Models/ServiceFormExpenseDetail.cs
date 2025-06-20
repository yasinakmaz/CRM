namespace CRM.Models
{
    [Table("SERVICEFORMEXPENSEDETAIL")]
    [Index(nameof(FORMIND), Name = "IX_ServiceFormExpenseDetail_FormInd")]
    [Index(nameof(CREATEDDATE), Name = "IX_ServiceFormExpenseDetail_CreatedDate")]
    public class ServiceFormExpenseDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        [Required]
        public int FORMIND { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999999999.99, ErrorMessage = "Tutar 0.01 ile 999,999,999.99 arasında olmalıdır")]
        public decimal AMOUNT { get; set; }

        [MaxLength(500)]
        public string? COMMENT { get; set; }

        [Required]
        public int CREATORUSERIND { get; set; } = 1;

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CREATEDDATE { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime LASTUPDATEDATE { get; set; } = DateTime.UtcNow;

        [Required]
        public int LASTUPDATEUSERIND { get; set; } = 1;

        public virtual ServiceFormHeader? ServiceFormHeader { get; set; }

        [NotMapped]
        public string AmountFormatted => AMOUNT.ToString("C2", CultureInfo.CurrentCulture);

        [NotMapped]
        public string CommentDisplay => string.IsNullOrWhiteSpace(COMMENT) ? "Açıklama yok" : COMMENT;

        [NotMapped]
        public string CreatedDateFormatted => CREATEDDATE.ToString("dd.MM.yyyy HH:mm");

        public bool IsValid()
        {
            return AMOUNT > 0 && !string.IsNullOrWhiteSpace(COMMENT);
        }

        public override string ToString()
        {
            return $"{AmountFormatted} - {CommentDisplay}";
        }

        public override bool Equals(object? obj)
        {
            return obj is ServiceFormExpenseDetail detail && IND == detail.IND;
        }

        public override int GetHashCode()
        {
            return IND.GetHashCode();
        }
    }
}
