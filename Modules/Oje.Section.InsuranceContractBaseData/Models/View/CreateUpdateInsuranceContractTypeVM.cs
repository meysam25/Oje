using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractTypeVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
