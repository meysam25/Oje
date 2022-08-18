using Oje.Infrastructure.Models;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabTypeFieldVehicleSpecService
    {
        ApiResult Create(SanabTypeFieldVehicleSpecCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        object Update(SanabTypeFieldVehicleSpecCreateUpdateVM input);
        GridResultVM<SanabTypeFieldVehicleSpecMainGridResultVM> GetList(SanabTypeFieldVehicleSpecMainGrid searchInput);
        int? GetSpecId(string title);
        VehicleSpec GetSpec(string title);
    }
}
