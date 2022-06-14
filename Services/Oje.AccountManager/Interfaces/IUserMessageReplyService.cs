
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface IUserMessageReplyService
    {
        void Create(UserMessageCreateVM input, long? loginUserId, int? siteSettingId, long? userMessageId);
        object GetList(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId);
        object Create(UserMessageCreateVM input, int? siteSettingId, long? loginUserId);
    }
}
