using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Interfaces
{
    public interface IVehicleUsageManager
    {
        ApiResult Create(CreateUpdateVehicleUsageVM input);
        ApiResult Delete(int? id);
        CreateUpdateVehicleUsageVM GetById(int? id);
        ApiResult Update(CreateUpdateVehicleUsageVM input);
        GridResultVM<VehicleUsageMainGridResultVM> GetList(VehicleUsageMainGrid searchInput);
    }
}
