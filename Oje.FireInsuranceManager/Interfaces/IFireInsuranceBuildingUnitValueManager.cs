using Oje.FireInsuranceService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Interfaces
{
    public interface IFireInsuranceBuildingUnitValueService
    {
        object GetLightList();
        FireInsuranceBuildingUnitValue GetById(int? id);
    }
}
