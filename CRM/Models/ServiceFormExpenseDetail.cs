namespace CRM.Models
{
    public class ServiceFormExpenseDetail
    {
        [Key]
        public int IND { get; set; }
        public int FORMIND { get; set; }
        [Column(TypeName = "money")]
        public decimal AMOUNT { get; set; }
        public string? COMMENT { get; set; }
        public int CREATORUSERIND { get; set; }
        public DateTime CREATEDDATE { get; set; }
        public DateTime LASTUPDATEDATE { get; set; }
        public int LASTUPDATEUSERIND { get; set; }
    }
}
