using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.Secretariat.Models.View;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface ISecretariatLetterUserService
    {
        long? Create(long secretariatLetterId, string mobile, string fullname, int siteSettingId, long userId, SecretariatLetterUserType type);
        ApiResult CreateForWeb(SecretariatLetterUserCreateVM input, int? siteSettingId, long? userId);
        ApiResult Delete(long? secretariatLetterId, long? id, int? siteSettingId, long? userId);
        object GetList(SecretariatLetterUserMainGrid searchInput, int? siteSettingId, long? loginUserId);
        void Update(long secretariatLetterId, string mobile, string fullname, int siteSettingId, long userId, SecretariatLetterUserType type);
    }
}
