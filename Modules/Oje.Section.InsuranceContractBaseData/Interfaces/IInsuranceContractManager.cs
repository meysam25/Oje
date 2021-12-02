using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractManager
    {
        ApiResult Create(CreateUpdateInsuranceContractVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractVM input);
        GridResultVM<InsuranceContractMainGridResultVM> GetList(InsuranceContractMainGrid searchInput);
        bool Exist(int id, int? siteSettingId, List<long> userChilds);
        object GetLightList();
    }
}
