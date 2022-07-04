using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IAgentRefferService
    {
        AgentReffer GetBy(int? companyId, int? siteSettingId);
    }
}
