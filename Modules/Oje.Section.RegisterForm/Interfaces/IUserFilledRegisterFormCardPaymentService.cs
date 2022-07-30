using Oje.Infrastructure.Models;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserFilledRegisterFormCardPaymentService
    {
        ApiResult Create(int? siteSettingId, UserFilledRegisterFormCardPaymentCreateUpdateVM input, long? loginUserId);
        object GetList(UserRegisterFormPaymentMainGrid searchInput, int? siteSettingId, bool? isPayed, bool? isDone);
        ApiResult Delete(long? id, int? siteSettingId, bool? isPayed, bool? isDone);
    }
}
