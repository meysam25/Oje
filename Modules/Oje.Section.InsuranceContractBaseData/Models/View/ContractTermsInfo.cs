using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class ContractTermsInfo
    {
        public string termsDescription { get; internal set; }
        public string firstName { get; internal set; }
        public string lastName { get; internal set; }
        public string nationalCode { get; internal set; }
        public string mobile { get; internal set; }
        public string contractDocumentUrl { get; internal set; }
    }
}
