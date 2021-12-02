using Oje.FireInsuranceManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Interfaces
{
    public interface IGlobalDiscountManager
    {
        List<GlobalDiscount> GetAutoDiscountList(int? proposalFormId, int? siteSettingId);
        GlobalDiscount GetByCode(string discountCode, int? siteSettingId, int? proposalFormId);
    }
}
