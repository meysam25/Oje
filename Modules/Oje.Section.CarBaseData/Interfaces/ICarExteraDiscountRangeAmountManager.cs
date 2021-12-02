using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarExteraDiscountRangeAmountManager
    {
        ApiResult Create(CreateUpdateCarExteraDiscountRangeAmountVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarExteraDiscountRangeAmountVM GetById(int? id);
        ApiResult Update(CreateUpdateCarExteraDiscountRangeAmountVM input);
        GridResultVM<CarExteraDiscountRangeAmountMainGridResultVM> GetList(CarExteraDiscountRangeAmountMainGrid searchInput);
    }
}
