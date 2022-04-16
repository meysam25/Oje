using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class ExternalNotificationServiceConfigMainGrid: GlobalGrid
    {
        public string subject { get; set; }
        public bool? isActive { get; set; }
        public ExternalNotificationServiceConfigType? type { get; set; }
    }
}
