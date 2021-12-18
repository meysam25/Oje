using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface ICashPayDiscountService
    {
        ApiResult Create(CreateUpdateCashPayDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateCashPayDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateCashPayDiscountVM input);
        GridResultVM<CashPayDiscountMainGridResultVM> GetList(CashPayDiscountMainGrid searchInput);
    }
}
