using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractCompanyService
    {
        ApiResult Create(CreateUpdateInsuranceContractCompanyVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractCompanyVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractCompanyVM input);
        GridResultVM<InsuranceContractCompanyMainGridResultVM> GetList(InsuranceContractCompanyMainGrid searchInput);
        bool Exist(int id, int? siteSettingId, List<long> childUserIds);
        object GetLightList();
    }
}
