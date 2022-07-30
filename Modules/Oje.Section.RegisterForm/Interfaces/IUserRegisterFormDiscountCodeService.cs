using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserRegisterFormDiscountCodeService
    {
        ApiResult Create(UserRegisterFormDiscountCodeCreateUpdateVM input, int? siteSettingId);
        object GetBy(long? id, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        ApiResult Update(UserRegisterFormDiscountCodeCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserRegisterFormDiscountCodeMainGridResultVM> GetList(UserRegisterFormDiscountCodeMainGrid searchInput, int? siteSettingId);
        UserRegisterFormDiscountCode GetBy(string code, int? siteSettingId, int formId);
        void DiscountUsed(int id, long? userId, long userFilledRegisterFormId);
    }
}
