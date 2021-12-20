using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IThirdPartyFinancialAndBodyHistoryDamagePenaltyService
    {
        ThirdPartyFinancialAndBodyHistoryDamagePenalty GetById(int? id, bool isFinancial);
        object GetLightListForBody();
        object GetLightListForFinancial();
    }
}
