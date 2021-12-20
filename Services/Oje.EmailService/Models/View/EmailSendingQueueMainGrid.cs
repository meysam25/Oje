using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.View
{
    public class EmailSendingQueueMainGrid : GlobalGrid
    {
        public UserNotificationType? type { get; set; }
        public string email { get; set; }
        public string createDate { get; set; }
        public string lTryDate { get; set; }
        public int? countTry { get; set; }
        public bool? isSuccess { get; set; }
        public string ip { get; set; }
    }
}
