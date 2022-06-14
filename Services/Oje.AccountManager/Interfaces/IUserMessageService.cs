using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface IUserMessageService
    {
        ApiResult Create(UserMessageCreateVM input, long? loginUserId, int? siteSettingId);
        GridResultVM<UserMessageMainGridResultVM> GetList(UserMessageMainGrid searchInput, long? loginUserId, int? siteSettingId);
    }
}
