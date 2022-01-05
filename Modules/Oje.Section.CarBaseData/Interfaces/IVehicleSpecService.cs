using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface IVehicleSpecService
    {
        ApiResult Create(VehicleSpecCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(VehicleSpecCreateUpdateVM input);
        GridResultVM<VehicleSpecMainGridResultVM> GetList(VehicleSpecMainGrid searchInput);
        object GetLightList(int? VehicleSpecCategoryId, int? VehicleSystemId, Select2SearchVM searchInput);
    }
}
