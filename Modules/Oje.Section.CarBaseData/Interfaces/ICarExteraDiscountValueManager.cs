using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarExteraDiscountValueService
    {
        ApiResult Create(CreateUpdateCarExteraDiscountValueVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarExteraDiscountValueVM GetById(int? id);
        ApiResult Update(CreateUpdateCarExteraDiscountValueVM input);
        GridResultVM<CarExteraDiscountValueMainGridResultVM> GetList(CarExteraDiscountValueMainGrid searchInput);
        object GetLightListByCarExteraDiscountId(int carExteraDiscountId);
    }
}
