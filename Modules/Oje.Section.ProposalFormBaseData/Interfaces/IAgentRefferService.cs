using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IAgentRefferService
    {
        ApiResult Create(AgentRefferCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(AgentRefferCreateUpdateVM input, int? siteSettingId);
        GridResultVM<AgentRefferMainGridResultVM> GetList(AgentRefferMainGrid searchInput, int? siteSettingId);
    }
}
