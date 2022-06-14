using Oje.Infrastructure.Models;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface IProposalFormPrintDescrptionService
    {
        ApiResult Create(ProposalFormPrintDescrptionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        ProposalFormPrintDescrptionCreateUpdateVM GetById(long? id, int? siteSettingId);
        ApiResult Update(ProposalFormPrintDescrptionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<ProposalFormPrintDescrptionMainGridResultVM> GetList(ProposalFormPrintDescrptionMainGrid searchInput, int? siteSettingId);
    }
}
