using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormRequiredDocumentMainGrid: GlobalGrid
    {
        public string type { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public bool? isRequired { get; set; }
    }
}
