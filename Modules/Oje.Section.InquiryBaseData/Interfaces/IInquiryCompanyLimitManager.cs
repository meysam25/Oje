using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInquiryCompanyLimitManager
    {
        ApiResult Create(CreateUpdateInquiryCompanyLimitVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateInquiryCompanyLimitVM input);
        GridResultVM<InquiryCompanyLimitMainGridResult> GetList(InquiryCompanyLimitMainGrid searchInput);
    }
}
