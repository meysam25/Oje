using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface INoDamageDiscountManager
    {
        ApiResult Create(CreateUpdateNoDamageDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateNoDamageDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateNoDamageDiscountVM input);
        GridResultVM<NoDamageDiscountMainGridResultVM> GetList(NoDamageDiscountMainGrid searchInput);
    }
}
