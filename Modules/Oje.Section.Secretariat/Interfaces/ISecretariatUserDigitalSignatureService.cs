using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatUserDigitalSignatureService
    {
        ApiResult Create(SecretariatUserDigitalSignatureCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        object GetById(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        GridResultVM<SecretariatUserDigitalSignatureMainGridResultVM> GetList(SecretariatUserDigitalSignatureMainGrid searchInput, int? siteSettingId);
        ApiResult Update(SecretariatUserDigitalSignatureCreateUpdateVM input, int? siteSettingId);
    }
}
