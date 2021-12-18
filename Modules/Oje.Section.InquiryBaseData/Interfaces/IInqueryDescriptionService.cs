using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInqueryDescriptionService
    {
        ApiResult Create(CreateUpdateInqueryDescriptionVM input);
        ApiResult Delete(int? id);
        CreateUpdateInqueryDescriptionVM GetById(int? id);
        ApiResult Update(CreateUpdateInqueryDescriptionVM input);
        GridResultVM<InqueryDescriptionMainGridResultVM> GetList(InqueryDescriptionMainGrid searchInput);
    }
}
