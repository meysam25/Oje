using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormJsonService
    {
        void Create(long proposalFilledFormId, string jsonConfig);
        ProposalFilledFormJson GetBy(long proposalFilledFormId);
        ProposalFilledFormCacheJson GetCacheBy(long id);
    }
}
