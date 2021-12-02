using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarExteraDiscountManager
    {
        ApiResult Create(CreateUpdateCarExteraDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarExteraDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateCarExteraDiscountVM input);
        GridResultVM<CarExteraDiscountMainGridResultVM> GetList(CarExteraDiscountMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
