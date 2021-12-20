using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IInquiryMaxDiscountService
    {
        List<InquiryMaxDiscount> GetByFormAndSiteSettingId(int? proposalFormId, int? siteSettingId);
    }
}
