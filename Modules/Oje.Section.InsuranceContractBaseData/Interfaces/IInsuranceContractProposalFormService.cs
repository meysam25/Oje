using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFormService
    {
        ApiResult Create(InsuranceContractProposalFormCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        InsuranceContractProposalFormCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(InsuranceContractProposalFormCreateUpdateVM input,  int? siteSettingId);
        GridResultVM<InsuranceContractProposalFormMainGridResultVM> GetList(InsuranceContractProposalFormMainGrid searchInput, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int id, int? siteSettingId);
    }
}
