using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatLetterCategoryService
    {
        ApiResult Create(SecretariatLetterCategoryCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        object GetById(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        GridResultVM<SecretariatLetterCategoryMainGridResultVM> GetList(SecretariatLetterCategoryMainGrid searchInput, int? siteSettingId);
        ApiResult Update(SecretariatLetterCategoryCreateUpdateVM input, int? siteSettingId);
    }
}
