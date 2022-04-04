using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormJsonService
    {
        void Create(long insuranceContractProposalFilledFormId, string jsonConfigStr);
        string GetBy(long insuranceContractProposalFilledFormId);
    }
}
