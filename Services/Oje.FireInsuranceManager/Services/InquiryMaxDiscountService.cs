using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class InquiryMaxDiscountService : IInquiryMaxDiscountService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public InquiryMaxDiscountService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public List<InquiryMaxDiscount> GetByFormId(int? proposalFormId, int? siteSettingId)
        {
            return 
                db.InquiryMaxDiscounts
                .Include(t => t.InquiryMaxDiscountCompanies)
                .Where(t => t.IsActive == true && t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId && t.IsActive == true)
                .AsNoTracking()
                .ToList();
        }
    }
}
