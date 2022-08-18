using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.View;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormReminderService
    {
        ApiResult Create(ReminderCreateVM input, int? siteSettingId, IpSections ipSections, long? loginUserId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        ApiResult Update(ReminderCreateVM input, int? siteSettingId, long? userId);
        GridResultVM<ProposalFormReminderMainGridResultVM> GetList(ProposalFormReminderMainGrid searchInput, int? siteSettingId);
    }
}
