using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class CreateUpdateIpLimitationBlackListVM
    {
        public int? id { get; set; }
        public string ip { get; set; }
        public bool? isActive { get; set; }
    }
}
