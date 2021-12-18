using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormPostPriceService
    {
        ApiResult Create(CreateUpdateProposalFormPostPriceVM input);
        ApiResult Delete(int? id);
        CreateUpdateProposalFormPostPriceVM GetById(int? id);
        ApiResult Update(CreateUpdateProposalFormPostPriceVM input);
        GridResultVM<ProposalFormPostPriceMainGridResultVM> GetList(ProposalFormPostPriceMainGrid searchInput);
    }
}
