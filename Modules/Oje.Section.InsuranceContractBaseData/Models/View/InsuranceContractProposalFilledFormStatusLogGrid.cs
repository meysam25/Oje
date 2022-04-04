using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormStatusLogGrid: GlobalGridParentLong
    {
        public InsuranceContractProposalFilledFormType? status { get; set; }
        public string userFullname { get; set; }
        public string createDate { get; set; }
        public string desc { get; set; }
    }
}
