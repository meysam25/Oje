using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserNotificationMainGrid: GlobalGrid
    {
        public string createDate { get; set; }
        public UserNotificationType? type { get; set; }
        public string fromUser { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string viewDate { get; set; }
        public bool? justMyNotification { get; set; }
        public string toUser { get; set; }
        public bool? notSeen { get; set; }
    }
}
