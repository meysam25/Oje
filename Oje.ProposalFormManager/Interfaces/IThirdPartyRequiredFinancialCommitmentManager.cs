using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IThirdPartyRequiredFinancialCommitmentManager
    {
        List<ThirdPartyRequiredFinancialCommitment> GetByIds(List<int> coverIds);
        object GetLightList();
    }
}
