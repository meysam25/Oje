using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IAutoAnswerOnlineChatMessageService
    {
        ApiResult Create(AutoAnswerOnlineChatMessageCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(AutoAnswerOnlineChatMessageCreateUpdateVM input, int? siteSettingId);
        GridResultVM<AutoAnswerOnlineChatMessageMainGridResultVM> GetList(AutoAnswerOnlineChatMessageMainGrid searchInput, int? siteSettingId);
        object GetByParentId(int? parentId, int? siteSettingId);
        Task LikeOrDislike(int? id, bool isLike, int? siteSettingId, IpSections clientIp);
    }
}
