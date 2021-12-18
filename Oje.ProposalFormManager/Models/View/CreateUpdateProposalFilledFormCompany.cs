using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class CreateUpdateProposalFilledFormCompany
    {
        public long? id { get; set; }
        public List<int> cIds { get; set; }
    }
}
