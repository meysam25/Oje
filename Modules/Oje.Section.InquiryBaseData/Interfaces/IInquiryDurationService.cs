using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInquiryDurationService
    {
        ApiResult Create(CreateUpdateInquiryDurationVM input);
        ApiResult Delete(int? id);
        CreateUpdateInquiryDurationVM GetById(int? id);
        ApiResult Update(CreateUpdateInquiryDurationVM input);
        GridResultVM<InquiryDurationMainGridResultVM> GetList(InquiryDurationMainGrid searchInput);
    }
}
