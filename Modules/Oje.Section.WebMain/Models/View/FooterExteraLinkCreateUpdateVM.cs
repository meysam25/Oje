using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterExteraLinkCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
    }
}
