using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdateProposalFormRequiredDocumentTypeVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? formId { get; set; }
        public string formId_Title { get; set; }
        public bool? isActive { get; set; }
        public int? siteId { get; set; }
    }
}
