using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface IVehicleTypeService
    {
        ApiResult Create(CreateUpdateVehicleTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateVehicleTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateVehicleTypeVM input);
        GridResultVM<VehicleTypeMainGridResultVM> GetList(VehicleTypeMainGrid searchInput);
    }
}
