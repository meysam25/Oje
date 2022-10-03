using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            BankAccounts = new();
            BankAccountSadads = new();
            BankAccountSeps = new();
            BankAccountSizpaies = new();
        }

        [Key]
        public int Id { get; set;}
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<BankAccount> BankAccounts { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSadad> BankAccountSadads { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSep> BankAccountSeps { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BankAccountSizpay> BankAccountSizpaies { get; set; }
    }
}
