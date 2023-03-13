using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("BankAccountSadads")]
    public class BankAccountSadad: SignatureEntity, IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId"), InverseProperty("BankAccountSadads")]
        public BankAccount BankAccount { get; set; }
        [Required, MaxLength(50)]
        public string MerchantId { get; set; }
        [Required, MaxLength(50)]
        public string TerminalId { get; set; }
        [Required, MaxLength(100)]
        public string TerminalKey { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BankAccountSadads")]
        public SiteSetting SiteSetting { get; set; }
    }
}
