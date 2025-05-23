﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class UserAdminLogConfigService : IUserAdminLogConfigService
    {
        readonly SecurityDBContext db = null;
        static List<UserAdminLogConfig> userAdminLogConfigs = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly IActionService ActionService = null;

        public UserAdminLogConfigService
            (
                SecurityDBContext db,
                IHttpContextAccessor HttpContextAccessor,
                IActionService ActionService
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
            this.ActionService = ActionService;
        }

        public ApiResult Create(UserAdminLogConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            db.Entry(new UserAdminLogConfig()
            {
                ActionId = input.aId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            userAdminLogConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserAdminLogConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.aId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (db.UserAdminLogConfigs.Any(t => t.ActionId == input.aId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.UserAdminLogConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            userAdminLogConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.UserAdminLogConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    aId = t.ActionId,
                    aId_Title = t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title,
                    isActive = t.IsActive,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<UserAdminLogConfigMainGridResultVM> GetList(UserAdminLogConfigMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserAdminLogConfigMainGrid();

            var quiryResult = db.UserAdminLogConfigs.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => (t.Action.Controller.Title + "/" + t.Action.Title).Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<UserAdminLogConfigMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new UserAdminLogConfigMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(UserAdminLogConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.UserAdminLogConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.ActionId = input.aId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            userAdminLogConfigs = null;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool IsNeededCache(long actionId, int siteSettingId)
        {
            if (userAdminLogConfigs == null)
                userAdminLogConfigs = db.UserAdminLogConfigs.AsNoTracking().ToList();

            return userAdminLogConfigs.Any(t => t.ActionId == actionId && t.IsActive == true && t.SiteSettingId == siteSettingId);
        }

        public ApiResult CreateAll(int? siteSettingId)
        {
            var allAction = ActionService.GetList();

            foreach(var action in allAction)
            {
                try { Create(new UserAdminLogConfigCreateUpdateVM() 
                {
                    aId = action,
                    isActive = true
                }, siteSettingId); } catch { };
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
