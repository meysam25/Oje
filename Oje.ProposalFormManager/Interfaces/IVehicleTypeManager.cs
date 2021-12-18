using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IVehicleTypeService
    {
        VehicleType GetById(int? id);
        object GetLightList(int? vehicleSystemId);
    }
}
