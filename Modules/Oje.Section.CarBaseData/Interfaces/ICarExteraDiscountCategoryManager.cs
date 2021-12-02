using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarExteraDiscountCategoryManager
    {
        ApiResult Create(CreateUpdateCarExteraDiscountCategoryVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarExteraDiscountCategoryVM GetById(int? id);
        ApiResult Update(CreateUpdateCarExteraDiscountCategoryVM input);
        GridResultVM<CarExteraDiscountCategoryMainGridResult> GetList(CarExteraDiscountCategoryMainGrid searchInput);
        object GetLightList();
    }
}
