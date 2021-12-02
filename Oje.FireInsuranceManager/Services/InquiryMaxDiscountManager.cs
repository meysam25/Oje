using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class InquiryMaxDiscountManager : IInquiryMaxDiscountManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public InquiryMaxDiscountManager(FireInsuranceManagerDBContext db)
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
