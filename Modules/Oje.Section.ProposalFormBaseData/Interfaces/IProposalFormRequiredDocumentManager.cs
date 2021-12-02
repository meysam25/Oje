using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormRequiredDocumentManager
    {
        ApiResult Create(CreateUpdateProposalFormRequiredDocumentVM input, long? userId);
        ApiResult Delete(int? id);
        CreateUpdateProposalFormRequiredDocumentVM GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormRequiredDocumentVM input, long? userId);
        GridResultVM<ProposalFormRequiredDocumentMainGridResultVM> GetList(ProposalFormRequiredDocumentMainGrid searchInput);
    }
}
