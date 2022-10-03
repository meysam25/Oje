using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserRegisterFormCompanyService : IUserRegisterFormCompanyService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public UserRegisterFormCompanyService
            (
                RegisterFormDBContext db,
                IUserRegisterFormService UserRegisterFormService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserRegisterFormService = UserRegisterFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormCompanyCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new UserRegisterFormCompany()
            {
                CompanyId = input.cid.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                UserRegisterFormId = input.fid.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormCompanyCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!UserRegisterFormService.Exist(input.fid, siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (db.UserRegisterFormCompanies.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.UserRegisterFormId == input.fid && t.CompanyId == input.cid))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormCompanies
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterFormCompanies
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    fid = t.UserRegisterFormId,
                    cid = t.CompanyId,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormCompanyMainGridResultVM> GetList(UserRegisterFormCompanyMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UserRegisterFormCompanies.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.company))
                quiryResult = quiryResult.Where(t => t.Company.Title.Contains(searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.form))
                quiryResult = quiryResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.form));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));
            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormCompanyMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.Company.Title,
                    form = t.UserRegisterForm.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new UserRegisterFormCompanyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    company = t.company,
                    form = t.form,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormCompanyCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.UserRegisterFormCompanies
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.CompanyId = input.cid.Value;
            foundItem.UserRegisterFormId = input.fid.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? formId, int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = "" } };

            result.AddRange(db.UserRegisterFormCompanies.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.UserRegisterFormId == formId).Select(t => new
            {
                id = t.CompanyId,
                title = t.Company.Title
            }).ToList());

            return result;
        }

        public Company GetCompanyBy(int copmanyId, int? siteSettingId)
        {
            return db.UserRegisterFormCompanies
                .Where(t => t.CompanyId == copmanyId && t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => t.Company)
                .FirstOrDefault();
        }
    }
}
