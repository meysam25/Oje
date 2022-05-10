using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractTypeService
    {
        ApiResult Create(CreateUpdateInsuranceContractTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractTypeVM input);
        GridResultVM<InsuranceContractTypeMainGridResultVM> GetList(InsuranceContractTypeMainGrid searchInput);
        bool Exist(List<int> ids, int? siteSettingId, long? loginUserId);
        object GetLightList();
        object GetLightList(int? contractId);
        bool Exist(int? id, int? siteSettingId);
        object GetLightListBySiteSettingId(int? siteSettingId);
    }
}
