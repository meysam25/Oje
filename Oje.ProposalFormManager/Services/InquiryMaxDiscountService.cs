using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class InquiryMaxDiscountService: IInquiryMaxDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public InquiryMaxDiscountService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<InquiryMaxDiscount> GetByFormAndSiteSettingId(int? proposalFormId, int? siteSettingId)
        {
            return db.InquiryMaxDiscounts.Where(t => t.IsActive == true && t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId).ToList();
        }
    }
}
