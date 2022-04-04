using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class contractUserInput
    {
        public string mobile { get; set; }
        public string nationalCode { get; set; }
        public long? contractCode { get; set; }
        public string birthDate { get; set; }
    }
}
