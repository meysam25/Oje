using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class SiteMenueVM
    {
        public SiteMenueVM()
        {
            childs = new();
        }

        public string title { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public List<SiteMenueVM> childs { get; set; }
    }
}
