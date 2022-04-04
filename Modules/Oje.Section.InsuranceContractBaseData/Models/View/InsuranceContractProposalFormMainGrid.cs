using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFormMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
