using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormDetaileVM
    {
        public string ppfTitle { get; set; }
        public string ppfCreateDate { get; set; }
        public string id { get; set; }
        public string createUserFullname { get; set; }
        public List<ContractProposalFilledFormPdfGroupVM> ProposalFilledFormPdfGroupVMs { get; set; }
    }
}
