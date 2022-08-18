using Oje.Infrastructure.Models;

namespace Oje.Sanab.Interfaces
{
    public interface IVehicleSpecService
    {
        bool Exist(int VehicleSystemId, int id);
        object GetSelect2List(Select2SearchVM searchInput, int? vehicleSystemId);
    }
}
