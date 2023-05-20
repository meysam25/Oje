using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatHeaderFooterService
    {
        ApiResult Create(SecretariatHeaderFooterCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        object GetById(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        GridResultVM<SecretariatHeaderFooterMainGridResultVM> GetList(SecretariatHeaderFooterMainGrid searchInput, int? siteSettingId);
        ApiResult Update(SecretariatHeaderFooterCreateUpdateVM input, int? siteSettingId);
    }
}
