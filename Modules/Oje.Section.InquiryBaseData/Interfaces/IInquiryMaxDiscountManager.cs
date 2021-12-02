using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInquiryMaxDiscountManager
    {
        ApiResult Create(CreateUpdateInquiryMaxDiscountVM input);
        ApiResult Delete(int? id);
        CreateUpdateInquiryMaxDiscountVM GetById(int? id);
        ApiResult Update(CreateUpdateInquiryMaxDiscountVM input);
        GridResultVM<InquiryMaxDiscountMainGridResultVM> GetList(InquiryMaxDiscountMainGrid searchInput);
    }
}
