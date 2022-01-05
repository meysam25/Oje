using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IVehicleSpecsService
    {
        object GetSelect2List(Select2SearchVM searchInput, int? vehicleTypeId, int? brandId);
        VehicleSpec GetBy(int? id, int? brandId, int? vehicleTypeId);
    }
}
