using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class BlockLoginUserCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public bool? isActive { get; set; }
    }
}
