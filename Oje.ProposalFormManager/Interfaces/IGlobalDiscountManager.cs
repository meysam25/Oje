using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IGlobalDiscountManager
    {
        List<GlobalDiscount> GetAutoDiscounts(int? proposalFormId, int? siteSettingId);
        GlobalDiscount GetByCode(string discountCode, int? siteSettingId, int? proposalFormId);
    }
}
