using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsTrigerVM
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public int? roleId { get; set; }
        public long? userId { get; set; }
    }
}
