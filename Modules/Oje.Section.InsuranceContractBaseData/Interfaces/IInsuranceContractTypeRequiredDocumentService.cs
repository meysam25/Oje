using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractTypeRequiredDocumentService
    {
        ApiResult Create(InsuranceContractTypeRequiredDocumentCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        InsuranceContractTypeRequiredDocumentCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(InsuranceContractTypeRequiredDocumentCreateUpdateVM input, int? siteSettingId);
        GridResultVM<InsuranceContractTypeRequiredDocumentMainGridResultVM> GetList(InsuranceContractTypeRequiredDocumentMainGrid searchInput, int? siteSettingId);
        RequiredDocumentVM GetRequiredDocuments(int? insuranceContractId, int? insuranceContractTypeId, int? siteSettingId, string typeDescrtiption);
    }
}
