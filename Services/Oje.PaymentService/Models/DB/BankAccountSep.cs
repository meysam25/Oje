using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("BankAccountSeps")]
    public class BankAccountSep : IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId"), InverseProperty("BankAccountSeps")]
        public BankAccount BankAccount { get; set; }
        [Required, MaxLength(50)]
        public string TerminalId { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BankAccountSeps")]
        public SiteSetting SiteSetting { get; set; }
    }
}
