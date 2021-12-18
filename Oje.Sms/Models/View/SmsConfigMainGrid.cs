using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsConfigMainGrid: GlobalGrid
    {
        public string smsUsername { get; set; }
        public SmsConfigType? type { get; set; }
        public bool? isActive { get; set; }
    }
}
