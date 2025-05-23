﻿using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFilledFormStatusLogService : IInsuranceContractProposalFilledFormStatusLogService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public InsuranceContractProposalFilledFormStatusLogService
            (
                InsuranceContractBaseDataDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public void Create(long? InsuranceContractProposalFilledFormUserId, InsuranceContractProposalFilledFormType? status, DateTime now, long? loginUserId, string description)
        {
            if (InsuranceContractProposalFilledFormUserId.ToLongReturnZiro() > 0 && status != null && loginUserId.ToLongReturnZiro() > 0)
            {
                db.Entry(new InsuranceContractProposalFilledFormStatusLog()
                {
                    InsuranceContractProposalFilledFormUserId = InsuranceContractProposalFilledFormUserId.Value,
                    Status = status.Value,
                    CreateDate = now,
                    UserId = loginUserId.Value,
                    Description = description
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }
        }

        public object GetList(InsuranceContractProposalFilledFormStatusLogGrid searchInput, int? siteSettingId, InsuranceContractProposalFilledFormType status)
        {
            searchInput = searchInput ?? new InsuranceContractProposalFilledFormStatusLogGrid();

            var quiryResult = db.InsuranceContractProposalFilledForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.IsDelete != true && t.InsuranceContractProposalFilledFormUsers.Any(tt => tt.Status == status))
                .SelectMany(t => t.InsuranceContractProposalFilledFormUsers)
                .SelectMany(t => t.InsuranceContractProposalFilledFormStatusLogs)
                .Where(t => t.InsuranceContractProposalFilledFormUserId == searchInput.pKey && t.InsuranceContractProposalFilledFormUser.Status == status);
               
            if (searchInput.status != null)
                quiryResult = quiryResult.Where(t => t.Status == searchInput.status);
            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.desc))
                quiryResult = quiryResult.Where(t => t.Description.Contains(searchInput.desc));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.CreateDate,
                    status = t.Status,
                    userFullname = t.User.Firstname + " " + t.User.Lastname,
                    createDate = t.CreateDate,
                    desc = t.Description
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.id.Ticks,
                    status = t.status.GetEnumDisplayName(),
                    t.userFullname,
                    createDate = t.createDate.ToString("hh:MM") + " " + t.createDate.ToFaDate(),
                    desc = string.IsNullOrEmpty(t.desc) ? "" : t.desc
                })
                .ToList()
            };
        }

        public object GetListForUser(InsuranceContractProposalFilledFormStatusLogGrid searchInput, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> validStatus)
        {
            searchInput = searchInput ?? new InsuranceContractProposalFilledFormStatusLogGrid();

            var quiryResult = db.InsuranceContractProposalFilledFormStatusLogs
                .Where(t => 
                    t.InsuranceContractProposalFilledFormUserId == searchInput.pKey && t.InsuranceContractProposalFilledFormUser.InsuranceContractProposalFilledForm.SiteSettingId == siteSettingId && 
                    t.InsuranceContractProposalFilledFormUser.InsuranceContractProposalFilledForm.IsDelete != true && t.InsuranceContractProposalFilledFormUser.InsuranceContractProposalFilledForm.CreateUserId == loginUserId && 
                    validStatus.Contains(t.InsuranceContractProposalFilledFormUser.Status));

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.CreateDate,
                    status = t.Status,
                    createDate = t.CreateDate,
                    t.Description
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.id.Ticks,
                    status = t.status.GetEnumDisplayName(),
                    createDate = t.createDate.ToString("hh:MM") + " " + t.createDate.ToFaDate(),
                    description = !string.IsNullOrEmpty(t.Description) ? t.Description : ""
                })
                .ToList()
            };
        }
    }
}
