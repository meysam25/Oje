using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface ILoginDescrptionService
    {
        ApiResult Create(LoginDescrptionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        LoginDescrptionCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(LoginDescrptionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<LoginDescrptionMainGridResultVM> GetList(LoginDescrptionMainGrid searchInput, int? siteSettingId);
    }
}
