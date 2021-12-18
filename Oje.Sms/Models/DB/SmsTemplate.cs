using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("SmsTemplates")]
    public class SmsTemplate
    {
        [Key]
        public int Id { get; set; }
        public UserNotificationType Type { get; set; }
        [Required, MaxLength(100)]
        public string Subject { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public int SiteSettingId { get; set; }
    }
}
