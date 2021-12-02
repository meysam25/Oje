using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractValidUserForFullDebitManager
    {
        ApiResult Create(CreateUpdateInsuranceContractValidUserForFullDebitVM input);
        ApiResult Delete(long? id);
        CreateUpdateInsuranceContractValidUserForFullDebitVM GetById(long? id);
        ApiResult Update(CreateUpdateInsuranceContractValidUserForFullDebitVM input);
        GridResultVM<InsuranceContractValidUserForFullDebitMainGridResultVM> GetList(InsuranceContractValidUserForFullDebitMainGrid searchInput);
        ApiResult CreateFromExcel(GlobalExcelFile input);
    }
}
