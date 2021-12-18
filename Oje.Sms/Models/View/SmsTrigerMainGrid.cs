using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsTrigerMainGrid: GlobalGrid
    {
        public UserNotificationType? type { get; set; }
        public string roleName { get; set; }
        public string userName { get; set; }
        public bool? sendForOwner { get; set; }
    }
}
