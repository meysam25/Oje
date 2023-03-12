using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IUserRegisterFormPriceService
    {
        object GetBy(long? id);
        GridResultVM<UserRegisterFormPriceMainGridResultVM> GetList(UserRegisterFormPriceMainGrid searchInput);
    }
}
