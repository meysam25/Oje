using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormCommissionService
    {
        ApiResult Create(ProposalFormCommissionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id);
        object GetById(int? id);
        GridResultVM<ProposalFormCommissionMainGridResultVM> GetList(ProposalFormCommissionMainGrid searchInput);
        ApiResult Update(ProposalFormCommissionCreateUpdateVM input, int? siteSettingId);
    }
}
