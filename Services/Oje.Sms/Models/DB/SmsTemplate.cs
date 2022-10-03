using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("SmsTemplates")]
    public class SmsTemplate: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public UserNotificationType Type { get; set; }
        [Required, MaxLength(100)]
        public string Subject { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public ProposalFilledFormUserType? ProposalFilledFormUserType { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SmsTemplates")]
        public SiteSetting SiteSetting { get; set; }
    }
}
