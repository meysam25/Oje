using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatHeaderFooterDescriptionService
    {
        ApiResult Create(SecretariatHeaderFooterDescriptionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        object GetById(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        GridResultVM<SecretariatHeaderFooterDescriptionMainGridResultVM> GetList(SecretariatHeaderFooterDescriptionMainGrid searchInput, int? siteSettingId);
        ApiResult Update(SecretariatHeaderFooterDescriptionCreateUpdateVM input, int? siteSettingId);
    }
}
