using Oje.Infrastructure.Models;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabVehicleTypeService
    {
        ApiResult Create(SanabVehicleTypeCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(SanabVehicleTypeCreateUpdateVM input);
        GridResultVM<SanabVehicleTypeMainGridResultVM> GetList(SanabVehicleTypeMainGrid searchInput);
        int? GetTypeIdBy(string title);
    }
}
