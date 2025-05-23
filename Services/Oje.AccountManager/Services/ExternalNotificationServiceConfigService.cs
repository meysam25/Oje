﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class ExternalNotificationServiceConfigService : IExternalNotificationServiceConfigService
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        static Dictionary<int?, ExternalNotificationServiceConfig> cacheConfig = new Dictionary<int?, ExternalNotificationServiceConfig>();


        public ExternalNotificationServiceConfigService
            (
                AccountDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ExternalNotificationServiceConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            db.Entry(new ExternalNotificationServiceConfig()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                PrivateKey = input.prKey,
                PublicKey = input.puKey,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Subject = input.subject,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ExternalNotificationServiceConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.subject))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.subject.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.puKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_Public_Key);
            if (string.IsNullOrEmpty(input.prKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_Private_Key);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            ExternalNotificationServiceConfig foundItem = 
                db.ExternalNotificationServiceConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ExternalNotificationServiceConfigCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.ExternalNotificationServiceConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new ExternalNotificationServiceConfigCreateUpdateVM
                { 
                    id = t.Id,
                    isActive = t.IsActive,
                    prKey = t.PrivateKey,
                    puKey = t.PublicKey,
                    subject = t.Subject,
                    type = t.Type,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<ExternalNotificationServiceConfigMainGridResultVM> GetList(ExternalNotificationServiceConfigMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new ExternalNotificationServiceConfigMainGrid();

            var quiryResult = 
                db.ExternalNotificationServiceConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.subject))
                quiryResult = quiryResult.Where(t => t.Subject.Contains(searchInput.subject));
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;


            return new GridResultVM<ExternalNotificationServiceConfigMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new 
                    { 
                        id = t.Id,
                        subject = t.Subject,
                        isActive = t.IsActive,
                        type = t.Type,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new ExternalNotificationServiceConfigMainGridResultVM 
                    { 
                        row = ++row,
                        id = t.id,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        subject = t.subject,
                        type = t.type.GetEnumDisplayName(),
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(ExternalNotificationServiceConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            ExternalNotificationServiceConfig foundItem = 
                db.ExternalNotificationServiceConfigs
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.PrivateKey = input.prKey;
            foundItem.PublicKey = input.puKey;
            foundItem.Subject = input.subject;
            foundItem.Type = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ExternalNotificationServiceConfig GetActiveConfig(int? siteSettingId)
        {
            if (cacheConfig == null)
                cacheConfig = new Dictionary<int?, ExternalNotificationServiceConfig>();

            if (cacheConfig.Keys.Any(t => t == siteSettingId) && cacheConfig[siteSettingId] != null)
                return cacheConfig[siteSettingId];

            cacheConfig[siteSettingId] = db.ExternalNotificationServiceConfigs.AsNoTracking().Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();

            return cacheConfig[siteSettingId];
        }

        public List<ExternalNotificationServiceConfig> GetActiveConfig()
        {
            return db.ExternalNotificationServiceConfigs.AsNoTracking().Where(t => t.IsActive == true).ToList();
        }
    }
}
