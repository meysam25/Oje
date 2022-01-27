using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterGroupExteraLinkMainGrid: GlobalGrid
    {
        public int? pKey { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
