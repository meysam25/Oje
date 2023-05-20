using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatLetterReciveService
    {
        ApiResult Create(SecretariatLetterReciveCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult Delete(long? id, int? siteSettingId);
        object DeleteUploadFile(long? fileId, long? id, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        GridResultVM<SecretariatLetterReciveMainGridResultVM> GetList(SecretariatLetterReciveMainGrid searchInput, int? siteSettingId);
        object GetUploadFiles(GlobalGridParentLong input, int? siteSettingId);
        ApiResult Update(SecretariatLetterReciveCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult UploadNewFile(long? id, IFormFile mainFile, string title, int? siteSettingId, long? loginUserId);
    }
}
