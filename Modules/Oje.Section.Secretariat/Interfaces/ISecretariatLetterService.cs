using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatLetterService
    {
        ApiResult Confirm(long? id, int? siteSettingId);
        ApiResult Create(SecretariatLetterCreateUpdateVM input, int? siteSettingId, long? userId);
        ApiResult Delete(long? id, int? siteSettingId);
        object DeleteUploadFile(long? id, long? pKey, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        GridResultVM<SecretariatLetterMainGridResult> GetList(SecretariatLetterMainGrid searchInput, int? siteSettingId, long? loginUserId = null);
        object GetUploadFiles(GlobalGridParentLong input, int? siteSettingId, long? loginUserId);
        SecretariatLetterVM PdfDetailes(long id, int? siteSettingId, long? loginUserId);
        ApiResult Update(SecretariatLetterCreateUpdateVM input, int? siteSettingId, long? userId);
        object UploadNewFile(long? pKey, IFormFile mainFile, string title, int? siteSettingId, long? loginUserId);
    }
}
