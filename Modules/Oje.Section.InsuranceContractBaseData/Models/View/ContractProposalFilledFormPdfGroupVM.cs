using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class ContractProposalFilledFormPdfGroupVM
    {
        public string title { get; set; }
        public List<ContractProposalFilledFormPdfGroupItem> ContractProposalFilledFormPdfGroupItems { get; set; }
    }
}
