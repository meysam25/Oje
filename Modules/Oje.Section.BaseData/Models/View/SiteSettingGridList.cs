using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class SiteSettingGridList
    {
        public int id { get; set; }
        public string website { get; set; }
        public string title { get; set; }
        public string userfirstname { get; set; }
        public string userlastname { get; set; }
        public string isHttps { get; set; }
        public string isActive { get; set; }
        public int row { get; internal set; }
    }
}
