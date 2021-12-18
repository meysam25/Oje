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
    [Table("SmsConfigs")]
    public class SmsConfig
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public string Domain { get; set; }
        public SmsConfigType Type { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
