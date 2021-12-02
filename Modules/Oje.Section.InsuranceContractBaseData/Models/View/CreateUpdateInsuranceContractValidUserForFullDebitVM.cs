using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractValidUserForFullDebitVM
    {
        public long? id { get; set; }
        public int? insuranceContractId { get; set; }
        public string mobile { get; set; }
        public string nationalCode { get; set; }
        public int? countUse { get; set; }
        public bool isActive { get; set; }
    }
}
