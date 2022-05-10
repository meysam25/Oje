
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractInsuranceContractTypeMaxPriceService
    {
        ApiResult Create(InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(string strId, int? siteSettingId);
        object GetById(string strId, int? siteSettingId);
        ApiResult Update(InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input, int? siteSettingId);
        GridResultVM<InsuranceContractInsuranceContractTypeMaxPriceMainGridResultVM> GetList(InsuranceContractInsuranceContractTypeMaxPriceMainGrid searchInput, int? siteSettingId);
        long? GetMaxPrice(int? siteSettingId, int insuranceContractTypeId, int? insuranceContractId);
    }
}
