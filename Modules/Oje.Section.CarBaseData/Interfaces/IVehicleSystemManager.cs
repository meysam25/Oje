using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface IVehicleSystemManager
    {
        ApiResult Create(CreateUpdateVehicleSystemVM input);
        ApiResult Delete(int? id);
        CreateUpdateVehicleSystemVM GetById(int? id);
        ApiResult Update(CreateUpdateVehicleSystemVM input);
        GridResultVM<VehicleSystemMainGridResultVM> GetList(VehicleSystemMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
