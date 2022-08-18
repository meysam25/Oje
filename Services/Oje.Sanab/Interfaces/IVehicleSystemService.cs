using Oje.Infrastructure.Models;

namespace Oje.Sanab.Interfaces
{
    public interface IVehicleSystemService
    {
        bool Exist(int id);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
