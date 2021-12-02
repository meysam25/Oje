using Oje.FireInsuranceManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IRoundInqueryManager
    {
        RoundInquery GetBy(int? proposalFormId, int? siteSettingId);
    }
}
