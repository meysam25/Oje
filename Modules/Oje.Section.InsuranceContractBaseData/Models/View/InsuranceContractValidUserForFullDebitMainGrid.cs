using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractValidUserForFullDebitMainGrid : GlobalGrid
    {
        public int? contract { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
        public string mobile { get; set; }
        public string nationalCode { get; set; }
        public int? countUse { get; set; }
        public bool? isActive { get; set; }
    }
}
