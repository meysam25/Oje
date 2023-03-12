using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IUserService
    {
        object GetBy(long? id);
        GridResultVM<UserMainGridResultVM> GetList(UserMainGrid searchInput);
    }
}
