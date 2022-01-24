using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class TopMenuMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
        public long? pKey { get; set; }
    }
}
