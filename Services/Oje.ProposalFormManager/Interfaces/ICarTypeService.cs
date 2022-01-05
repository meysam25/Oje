using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface ICarTypeService
    {
        object GetLightList(int vehicleTypeId);
        CarType GetById(int? carTypeId, int? vehicleTypeId);
    }
}
