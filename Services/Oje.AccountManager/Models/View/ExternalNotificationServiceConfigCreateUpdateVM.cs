using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class ExternalNotificationServiceConfigCreateUpdateVM
    {
        public int? id { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString subject { get; set; }
        public string puKey { get; set; }
        public string prKey { get; set; }
        public bool? isActive { get; set; }
        public ExternalNotificationServiceConfigType? type { get; set; }
    }
}
