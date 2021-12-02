using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public bool? isActive { get; set; }
        public int? setting { get; set; }
    }
}
