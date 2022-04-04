using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractProposalFilledFormChangeStatusVM
    {
        public long? id { get; set; }
        public InsuranceContractProposalFilledFormType? status { get; set; }
        public string description { get; set; }
    }
}
