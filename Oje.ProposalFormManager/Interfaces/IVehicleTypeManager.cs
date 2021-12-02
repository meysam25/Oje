using Oje.ProposalFormManager.Models.DB;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IVehicleTypeManager
    {
        VehicleType GetById(int? id);
        object GetLightList(int? vehicleSystemId);
    }
}
