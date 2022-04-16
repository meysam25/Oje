using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class ExternalNotificationServicePushSubscriptionCreateVM
    {
        public string auth { get; set; }
        public string p256DH { get; set; }
        public string endpoint { get; set; }
    }
}
