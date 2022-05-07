using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractUserSubCategoryService
    {
        ApiResult Create(InsuranceContractUserSubCategoryCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(InsuranceContractUserSubCategoryCreateUpdateVM input, int? siteSettingId);
        GridResultVM<InsuranceContractUserSubCategoryMainGridResultVM> GetList(InsuranceContractUserSubCategoryMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        int? GetByCode(int? siteSettingId, string code);
    }
}
