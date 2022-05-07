using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormMainGrid: GlobalGrid
    {
        public string createUserfullname { get; set; }
        public string createDate { get; set; }
        public string confirmDate { get; set; }
        public string contractTitle { get; set; }
        public string familyMemers { get; set; }
    }
}
