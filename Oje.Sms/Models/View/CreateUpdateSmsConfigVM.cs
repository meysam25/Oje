using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsConfigVM
    {
        public int? id { get; set; }
        public string smsUsername { get; set; }
        public string smsPassword { get; set; }
        public string domain { get; set; }
        public SmsConfigType? type { get; set; }
        public bool? isActive { get; set; }
    }
}
