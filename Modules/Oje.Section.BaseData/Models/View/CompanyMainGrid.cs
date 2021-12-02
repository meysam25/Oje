using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class CompanyMainGrid: GlobalGrid
    {
        public string name { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
