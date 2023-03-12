using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IUserFilledRegisterFormService
    {
        object GetBy(long? id);
        GridResultVM<UserFilledRegisterFormMainGridResultVM> GetList(UserFilledRegisterFormMainGrid searchInput);
    }
}
