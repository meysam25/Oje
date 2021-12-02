using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractTypeManager
    {
        ApiResult Create(CreateUpdateInsuranceContractTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractTypeVM input);
        GridResultVM<InsuranceContractTypeMainGridResultVM> GetList(InsuranceContractTypeMainGrid searchInput);
        bool Exist(int id, int? siteSettingId, List<long> childUserIds);
        object GetLightList();
    }
}
