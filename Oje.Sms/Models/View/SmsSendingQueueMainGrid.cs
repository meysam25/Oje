using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsSendingQueueMainGrid: GlobalGrid
    {
        public UserNotificationType? type { get; set; }
        public string mobile { get; set; }
        public string createDate { get; set; }
        public string lTryDate { get; set; }
        public int? countTry { get; set; }
        public bool? isSuccess { get; set; }
        public string ip { get; set; }
    }
}
