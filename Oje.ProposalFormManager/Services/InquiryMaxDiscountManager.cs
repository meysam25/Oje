using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class InquiryMaxDiscountManager: IInquiryMaxDiscountManager
    {
        readonly ProposalFormDBContext db = null;
        public InquiryMaxDiscountManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<InquiryMaxDiscount> GetByFormAndSiteSettingId(int? proposalFormId, int? siteSettingId)
        {
            return db.InquiryMaxDiscounts.Where(t => t.IsActive == true && t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId).ToList();
        }
    }
}
