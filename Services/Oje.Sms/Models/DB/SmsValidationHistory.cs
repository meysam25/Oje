using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("SmsValidationHistories")]
    public class SmsValidationHistory
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
    }
}
