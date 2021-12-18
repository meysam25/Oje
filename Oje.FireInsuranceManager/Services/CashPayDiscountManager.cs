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
    public class CashPayDiscountService: ICashPayDiscountService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public CashPayDiscountService(FireInsuranceServiceDBContext db)
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
