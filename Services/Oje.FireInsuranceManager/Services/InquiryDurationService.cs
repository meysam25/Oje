﻿using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class InquiryDurationService : IInquiryDurationService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public InquiryDurationService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public InquiryDuration GetBy(int? siteSettingId, int? proposalFormId, int? id)
        {
            return db.InquiryDurations
                .Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.Id == id)
                .Include(t => t.InquiryDurationCompanies)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetLightList(int? siteSettingId, int? proposalFormId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.InquiryDurations
                .Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId && t.IsActive == true)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }
    }
}
