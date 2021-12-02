using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IGlobalDiscountManager
    {
        ApiResult Create(CreateUpdateGlobalDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateGlobalDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateGlobalDiscountVM input);
        GridResultVM<GlobalDiscountMainGridResult> GetList(GlobalDiscountMainGrid searchInput);
    }
}
