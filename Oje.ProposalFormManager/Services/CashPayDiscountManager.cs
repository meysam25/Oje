using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class CashPayDiscountManager: ICashPayDiscountManager
    {
        readonly ProposalFormDBContext db = null;
        public CashPayDiscountManager(ProposalFormDBContext db)
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
