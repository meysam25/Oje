using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInsuranceContractDiscountService
    {
        ApiResult Create(CreateUpdateInsuranceContractDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractDiscountVM input);
        GridResultVM<InsuranceContractDiscountMainGridResultVM> GetList(InsuranceContractDiscountMainGrid searchInput);
    }
}
