using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IProposalFilledFormService
    {
        object GetBy(long? id);
        GridResultVM<ProposalFilledFormMainGridResultVM> GetList(ProposalFilledFormMainGrid searchInput);
    }
}
