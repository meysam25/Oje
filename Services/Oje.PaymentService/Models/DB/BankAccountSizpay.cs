using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("BankAccountSizpaies")]
    public class BankAccountSizpay
    {
        [Key]
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId"), InverseProperty("BankAccountSizpaies")]
        public BankAccount BankAccount { get; set; }
        [Required, MaxLength(50)]
        public string FirstKey { get; set; }
        [Required, MaxLength(50)]
        public string SecondKey { get; set; }
        [Required, MaxLength(50)]
        public string SignKey { get; set; }
        public long TerminalId { get; set; }
        public long MerchantId { get; set; }
        public int SiteSettingId { get; set; }
    }
}
