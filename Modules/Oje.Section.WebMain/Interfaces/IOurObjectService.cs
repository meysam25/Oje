using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IOurObjectService
    {
        ApiResult Create(OurCustomerCreateUpdateVM input, int? siteSettingId, OurObjectType type);
        ApiResult Delete(int? id, int? siteSettingId, OurObjectType type);
        object GetById(int? id, int? siteSettingId, OurObjectType type);
        ApiResult Update(OurCustomerCreateUpdateVM input, int? siteSettingId, OurObjectType type);
        GridResultVM<OurCustomerMainGridResultVM> GetList(OurCustomerMainGrid searchInput, int? siteSettingId, OurObjectType type);
        object GetListWeb(int? siteSettingId, OurObjectType type);
    }
}
