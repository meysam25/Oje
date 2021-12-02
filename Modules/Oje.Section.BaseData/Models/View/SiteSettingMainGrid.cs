using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class SiteSettingMainGrid: GlobalGrid
    {
        public string website { get; set; }
        public string title { get; set; }
        public string userfirstname { get; set; }
        public string userlastname { get; set; }
        public bool? isHttps { get; set; }
        public bool? isActive { get; set; }
    }
}
