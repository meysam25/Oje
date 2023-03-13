using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("BankAccountSeps")]
    public class BankAccountSep : SignatureEntity, IEntityWithSiteSettingId
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
