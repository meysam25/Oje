using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.View
{
    public class EmailTrigerMainGrid: GlobalGrid
    {
        public UserNotificationType? type { get; set; }
        public string roleName { get; set; }
        public string userName { get; set; }
    }
}
