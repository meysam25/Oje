﻿using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class InsuranceContractDiscountService: IInsuranceContractDiscountService
    {
        readonly ProposalFormDBContext db = null;
        public InsuranceContractDiscountService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public InsuranceContractDiscount GetById(int? siteSettingId, int? proposalFormId, int? id)
        {
            return db.InsuranceContractDiscounts
                .Include(t => t.InsuranceContractDiscountCompanies)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsActive == true && t.InsuranceContract.SiteSettingId == siteSettingId)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetLightList(int? siteSettingId, ProposalFormType ppfType)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.InsuranceContractDiscounts
                .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.InsuranceContract.ProposalForm.Type == ppfType)
                .Select(t => new { id = t.Id, title = t.Title })
                .ToList());

            return result;
        }
    }
}
