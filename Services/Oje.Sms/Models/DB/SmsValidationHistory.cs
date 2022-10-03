using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("SmsValidationHistories")]
    public class SmsValidationHistory: IEntityWithSiteSettingId
    {
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public SmsValidationHistoryType Type { get; set; }
        public long MobileNumber { get; set; }
        public int? ConfirmCode { get; set; }
        public int? InvalidCount { get; set; }
        public bool? IsUsed { get; set; }
        public bool? PreUsed { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SmsValidationHistories")]
        public SiteSetting SiteSetting { get; set; }
    }
}
