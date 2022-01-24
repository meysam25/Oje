using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class TopMenuCreateUpdateVM
    {
        public long? id { get; set; }
        public long? pKey { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }

    }
}
