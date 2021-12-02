using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateSiteSettingVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string websiteUrl { get; set; }
        public string panelUrl { get; set; }
        public long? userId { get; set; }
        public bool? isHttps { get; set; }
        public bool? isActive { get; set; }
    }
}
