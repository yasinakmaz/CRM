
namespace CRM.Models
{
    public class Business
    {
        [Key]
        public int IND { get; set; }
        public string? TYPE { get; set; }
        public string? BUSINESSNAME { get; set; }
        public bool BUSINESSTAXTYPE { get; set; }
        public string? TAXNUMBER { get; set; }
        public string? TAXOFFICE { get; set; }
        public string? AUTHNAMEANDSURNAME { get; set; }
        public string? PHONENUMBER { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime LASTUPDATE { get; set; }
        public int CREATORUSERIND { get; set; }
        public int LASTUPDATEUSERIND { get; set; }
    }
}
