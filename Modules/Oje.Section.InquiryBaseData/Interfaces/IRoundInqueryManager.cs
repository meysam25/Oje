using Oje.Infrastructure.Models;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IRoundInqueryService
    {
        ApiResult Create(CreateUpdateRoundInqueryVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateRoundInqueryVM input);
        GridResultVM<RoundInqueryMainGridResult> GetList(RoundInqueryMainGrid searchInput);
    }
}
