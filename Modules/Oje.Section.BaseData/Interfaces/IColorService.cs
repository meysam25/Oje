using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IColorService
    {
        ApiResult Create(ColorCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(ColorCreateUpdateVM input);
        GridResultVM<ColorMainGridResultVM> GetList(ColorMainGrid searchInput);
    }
}
