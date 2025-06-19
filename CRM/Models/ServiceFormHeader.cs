namespace CRM.Models
{
    public class ServiceFormHeader
    {
        [Key]
        public int IND { get; set; }
        public int TYPE { get; set; }
        public int BUSINESSIND { get; set; }
        public DateTime ServiceDate { get; set; }
        public int CreatorUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LASTUPDATEUSERIND { get; set; }
        public DateTime LASTUPDATEDATE { get; set; }
    }
}
