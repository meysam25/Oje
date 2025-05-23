﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormPrintDescrptionService : IProposalFormPrintDescrptionService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFormPrintDescrptionService
            (
                ProposalFormBaseDataDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ProposalFormPrintDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new ProposalFormPrintDescrption()
            {
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.pfid.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ProposalFormPrintDescrptionCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.pfid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!db.ProposalForms.Any(t => t.Id == input.pfid && (t.SiteSettingId == null || (t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (db.ProposalFormPrintDescrptions.Any(t => t.Id != input.id && t.ProposalFormId == input.pfid && t.Type == input.type && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.ProposalFormPrintDescrptions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ProposalFormPrintDescrptionCreateUpdateVM GetById(long? id, int? siteSettingId)
        {
            return db.ProposalFormPrintDescrptions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    t.Id,
                    t.ProposalFormId,
                    t.ProposalForm.Title,
                    t.Description,
                    t.Type,
                    t.IsActive,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormPrintDescrptionCreateUpdateVM
                {
                    id = t.Id,
                    description = t.Description,
                    isActive = t.IsActive,
                    pfid = t.ProposalFormId,
                    pfid_Title = t.Title,
                    type = t.Type,
                    cSOWSiteSettingId = t.cSOWSiteSettingId,
                    cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFormPrintDescrptionMainGridResultVM> GetList(ProposalFormPrintDescrptionMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new ProposalFormPrintDescrptionMainGrid();

            var quiryResult = db.ProposalFormPrintDescrptions.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.fTitle))
                quiryResult = quiryResult.Where(t => t.ProposalForm.Title.Contains(searchInput.fTitle));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormPrintDescrptionMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.Type,
                    pfTitle = t.ProposalForm.Title,
                    t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormPrintDescrptionMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    type = t.Type.GetEnumDisplayName(),
                    fTitle = t.pfTitle,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                }).ToList()
            };
        }

        public ApiResult Update(ProposalFormPrintDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.ProposalFormPrintDescrptions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.pfid.Value;
            foundItem.Type = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
