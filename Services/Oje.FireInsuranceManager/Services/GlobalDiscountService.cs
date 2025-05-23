﻿using Microsoft.EntityFrameworkCore;
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
    public class GlobalDiscountService : IGlobalDiscountService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public GlobalDiscountService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public List<GlobalDiscount> GetAutoDiscountList(int? proposalFormId, int? siteSettingId)
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
