using Oje.Infrastructure.Models;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabCarTypeService
    {
        ApiResult Create(SanabCarTypeCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(SanabCarTypeCreateUpdateVM input);
        GridResultVM<SanabCarTypeMainGridResultVM> GetList(SanabCarTypeMainGrid searchInput);
        int? GetTypeIdBy(long? code);
    }
}
