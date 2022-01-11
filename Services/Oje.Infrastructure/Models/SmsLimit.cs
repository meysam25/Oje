using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class SmsLimit
    {
        public UserNotificationType type { get; set; }
        public int value { get; set; }
        public bool? isWebsite { get; set; }
    }
}
