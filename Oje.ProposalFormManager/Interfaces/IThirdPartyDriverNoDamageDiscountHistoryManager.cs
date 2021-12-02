using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IThirdPartyDriverNoDamageDiscountHistoryManager
    {
        ThirdPartyDriverNoDamageDiscountHistory GetById(int? id);
        object GetLightList();
    }
}
