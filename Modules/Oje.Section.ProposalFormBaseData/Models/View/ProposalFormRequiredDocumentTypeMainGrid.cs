using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormRequiredDocumentTypeMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string form { get; set; }
        public int siteId { get; set; }
        public bool? isActive { get; set; }
    }
}
