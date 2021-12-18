using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class GlobalDiscountService : IGlobalDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public GlobalDiscountService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<GlobalDiscount> GetAutoDiscounts(int? proposalFormId, int? siteSettingId)
        {
            return db.GlobalDiscounts
                .Where(t =>
                            string.IsNullOrEmpty(t.Code) && t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now &&
                            t.SiteSettingId == siteSettingId && t.IsActive == true && t.MaxUse > t.GlobalDiscountUseds.Count() &&
                            t.ProposalFormId == proposalFormId
                            )
                .AsNoTracking()
                .ToList();
        }

        public GlobalDiscount GetByCode(string discountCode, int? siteSettingId, int? proposalFormId)
        {
            return db.GlobalDiscounts.Include(t => t.GlobalDiscountCompanies)
                    .Where(t =>
                            t.Code == discountCode && t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now &&
                            t.SiteSettingId == siteSettingId && t.IsActive == true && t.MaxUse > t.GlobalDiscountUseds.Count() &&
                            t.ProposalFormId == proposalFormId
                           )
                    .AsNoTracking()
                    .FirstOrDefault();
        }
    }
}
