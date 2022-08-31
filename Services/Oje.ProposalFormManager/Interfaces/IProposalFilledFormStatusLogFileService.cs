using Oje.ProposalFormService.Models.View;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormStatusLogFileService
    {
        void Create(long proposalFilledFormStatusLogId, ProposalFilledFormChangeStatusFileVM file, int? siteSettingId, long? userId);
    }
}
