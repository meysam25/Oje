using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class CreateUpdateSalesNetworkVM: GlobalSiteSetting
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<int> ppfIds { get; set; }
        public List<int> cIds { get; set; }
        public long? userId { get; set; }
        public string userId_Title { get; set; }
        public SalesNetworkType? type { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }


    }
}
