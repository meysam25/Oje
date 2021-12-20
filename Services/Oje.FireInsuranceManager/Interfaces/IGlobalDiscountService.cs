using Oje.FireInsuranceService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IGlobalDiscountService
    {
        List<GlobalDiscount> GetAutoDiscountList(int? proposalFormId, int? siteSettingId);
        GlobalDiscount GetByCode(string discountCode, int? siteSettingId, int? proposalFormId);
    }
}
