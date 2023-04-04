using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractUserBaseInsuranceService
    {
        ApiResult Create(InsuranceContractUserBaseInsuranceCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(InsuranceContractUserBaseInsuranceCreateUpdateVM input, int? siteSettingId);
        GridResultVM<InsuranceContractUserBaseInsuranceMainGridResultVM> GetList(InsuranceContractUserBaseInsuranceMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
        int? GetByCode(int? siteSettingId,string code);
        string GetIdByTitle(int? siteSettingId, string title);
    }
}
