using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface IVehicleSpecCategoryService
    {
        ApiResult Create(VehicleSpecCategoryCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(VehicleSpecCategoryCreateUpdateVM input);
        GridResultVM<VehicleSpecCategoryMainGridResultVM> GetList(VehicleSpecCategoryMainGrid searchInput);
        object GetLightList();
    }
}
