namespace CRM.Models
{
    [Table("BUSINESS")]
    [Index(nameof(BUSINESSNAME), Name = "IX_Business_BusinessName")]
    [Index(nameof(TAXNUMBER), Name = "IX_Business_TaxNumber")]
    [Index(nameof(CREATEDATE), Name = "IX_Business_CreateDate")]
    public class Business
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IND { get; set; }

        [MaxLength(50)]
        [Required]
        public string TYPE { get; set; } = string.Empty;

        [MaxLength(200)]
        [Required]
        public string BUSINESSNAME { get; set; } = string.Empty;

        [Required]
        public bool BUSINESSTAXTYPE { get; set; }

        [MaxLength(50)]
        public string? TAXNUMBER { get; set; }

        [MaxLength(100)]
        public string? TAXOFFICE { get; set; }

        [MaxLength(100)]
        public string? AUTHNAMEANDSURNAME { get; set; }

        [MaxLength(20)]
        public string? PHONENUMBER { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CREATEDATE { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime LASTUPDATE { get; set; } = DateTime.UtcNow;

        [Required]
        public int CREATORUSERIND { get; set; } = 1;

        [Required]
        public int LASTUPDATEUSERIND { get; set; } = 1;

        [NotMapped]
        public string TypeText => BUSINESSTAXTYPE ? "Tüzel" : "Gerçek";

        [NotMapped]
        public string DisplayName => $"{BUSINESSNAME} ({TypeText})";

        public override string ToString()
        {
            return $"{BUSINESSNAME} - {TYPE}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Business business && IND == business.IND;
        }

        public override int GetHashCode()
        {
            return IND.GetHashCode();
        }
    }
}