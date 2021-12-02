using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class CreateUpdateSalesNetworkVM
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<int> ppfIds { get; set; }
        public List<int> cIds { get; set; }
        public long? userId { get; set; }
        public string userId_Title { get; set; }
        public SalesNetworkType? type { get; set; }
        public PersonType? calceType { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }


    }
}
