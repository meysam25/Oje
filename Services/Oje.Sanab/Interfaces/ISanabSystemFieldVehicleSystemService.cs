using Oje.Infrastructure.Models;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabSystemFieldVehicleSystemService
    {
        ApiResult Create(SanabSystemFieldVehicleSystemCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(SanabSystemFieldVehicleSystemCreateUpdateVM input);
        GridResultVM<SanabSystemFieldVehicleSystemMainGridResultVM> GetList(SanabSystemFieldVehicleSystemMainGrid searchInput);
        int? GetSystemId(string title);
        VehicleSystem GetSystem(string title);
    }
}
