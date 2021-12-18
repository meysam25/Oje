using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class CashPayDiscountService: ICashPayDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public CashPayDiscountService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<CashPayDiscount> GetBySiteSettingAndProposalFormId(int? siteSettingId, int? ProposalFormId)
        {
            return db.CashPayDiscounts
                        .Include(t => t.CashPayDiscountCompanies)
                        .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.ProposalFormId == ProposalFormId)
                        .AsNoTracking()
                        .ToList();
        }

        public bool HasAnyDebitPayment(int? siteSettingId, int? ProposalFormId)
        {
            return db.CashPayDiscounts.Any(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == ProposalFormId);
        }
    }
}
