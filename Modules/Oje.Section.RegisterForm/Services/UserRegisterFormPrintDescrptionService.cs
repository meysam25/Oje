﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserRegisterFormPrintDescrptionService : IUserRegisterFormPrintDescrptionService
    {
        readonly RegisterFormDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserRegisterFormPrintDescrptionService
            (
                RegisterFormDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormPrintDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new UserRegisterFormPrintDescrption()
            {
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                UserRegisterFormId = input.pfid.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormPrintDescrptionCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.pfid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!db.UserRegisterForms.Any(t => t.Id == input.pfid && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (db.UserRegisterFormPrintDescrptions
                .Any(t => t.Id != input.id && t.UserRegisterFormId == input.pfid && t.Type == input.type && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormPrintDescrptions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public UserRegisterFormPrintDescrptionCreateUpdateVM GetById(long? id, int? siteSettingId)
        {
            return db.UserRegisterFormPrintDescrptions
               .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
               .Where(t => t.Id == id)
               .OrderByDescending(t => t.Id)
               .Take(1)
               .Select(t => new
               {
                   t.Id,
                   t.UserRegisterFormId,
                   t.Description,
                   t.Type,
                   t.IsActive,
                   cSOWSiteSettingId = t.SiteSettingId,
                   cSOWSiteSettingId_Title = t.SiteSetting.Title
               })
               .ToList()
               .Select(t => new UserRegisterFormPrintDescrptionCreateUpdateVM
               {
                   id = t.Id,
                   description = t.Description,
                   isActive = t.IsActive,
                   pfid = t.UserRegisterFormId,
                   type = t.Type,
                   cSOWSiteSettingId = t.cSOWSiteSettingId,
                   cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title
               })
               .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormPrintDescrptionMainGridResultVM> GetList(UserRegisterFormPrintDescrptionMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserRegisterFormPrintDescrptionMainGrid();

            var quiryResult = db.UserRegisterFormPrintDescrptions.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.fTitle))
                quiryResult = quiryResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.fTitle));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormPrintDescrptionMainGridResultVM>()
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
                    pfTitle = t.UserRegisterForm.Title,
                    t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title

                })
                .ToList()
                .Select(t => new UserRegisterFormPrintDescrptionMainGridResultVM
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

        public ApiResult Update(UserRegisterFormPrintDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.UserRegisterFormPrintDescrptions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.UserRegisterFormId = input.pfid.Value;
            foundItem.Type = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public string GetBy(int? siteSettingId, int formId, ProposalFormPrintDescrptionType type)
        {
            return db.UserRegisterFormPrintDescrptions
                .Where(t => t.SiteSettingId == siteSettingId && t.UserRegisterFormId == formId && t.Type == type && t.IsActive == true)
                .Select(t => t.Description)
                .FirstOrDefault();
        }
    }
}
