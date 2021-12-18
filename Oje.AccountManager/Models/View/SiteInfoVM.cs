using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class SiteInfoVM
    {
        public long? loginUserId { get; set; }
        public int? siteSettingId { get; set; }
        public List<long> childUserIds { get; set; }
    }
}
