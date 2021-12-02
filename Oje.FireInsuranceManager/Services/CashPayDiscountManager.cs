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
    public class CashPayDiscountManager: ICashPayDiscountManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public CashPayDiscountManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public List<CashPayDiscount> GetBy(int? proposalFormId, int? siteSettingId)
        {
            return db.CashPayDiscounts
                .Where(t => t.ProposalFormId == proposalFormId && t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Include(t => t.CashPayDiscountCompanies)
                .AsNoTracking()
                .ToList();
        }
    }
}
