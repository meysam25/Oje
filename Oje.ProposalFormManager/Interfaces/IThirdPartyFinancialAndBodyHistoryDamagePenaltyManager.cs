using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager
    {
        ThirdPartyFinancialAndBodyHistoryDamagePenalty GetById(int? id, bool isFinancial);
        object GetLightListForBody();
        object GetLightListForFinancial();
    }
}
