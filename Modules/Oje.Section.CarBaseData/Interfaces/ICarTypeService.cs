using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface ICarTypeService
    {
        ApiResult Create(CreateUpdateCarTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateCarTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateCarTypeVM input);
        GridResultVM<CarTypeMainGridResultVM> GetList(CarTypeMainGrid searchInput);
        object GetLightList();
    }
}
