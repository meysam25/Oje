﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System.Linq;

namespace Oje.Section.WebMain.Services
{
    public class TopMenuService : ITopMenuService
    {
        readonly WebMainDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TopMenuService
            (
                WebMainDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(TopMenuCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new TopMenu()
            {
                Title = input.title,
                ParentId = input.pKey,
                Order = input.order.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Link = input.link,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TopMenuCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.link) && input.link.Length > 1000)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.pKey.ToLongReturnZiro() > 0 && !db.TopMenus.Any(t => t.Id == input.pKey && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.TopMenus
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;

            try { db.SaveChanges(); } catch { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.TopMenus
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    link = t.Link,
                    isActive = t.IsActive,
                    order = t.Order,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<TopMenuMainGridResultVM> GetList(TopMenuMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new TopMenuMainGrid();

            var qureResult = db.TopMenus
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.ParentId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<TopMenuMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new TopMenuMainGridResultVM
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

        public ApiResult Update(TopMenuCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.TopMenus
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Link = input.link;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListForWeb(int? siteSettingId)
        {
            return db.TopMenus
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.ParentId == null)
                .OrderBy(t => t.Order)
                .Select(t => new
                {
                    title = t.Title,
                    link = t.Link,
                    childs = t.Childs
                        .Where(tt => tt.IsActive == true)
                        .OrderBy(tt => tt.Order)
                        .Select(tt => new
                        {
                            title = tt.Title,
                            link = tt.Link,
                            childs = tt.Childs
                                .Where(ttt => ttt.IsActive == true)
                                .OrderBy(ttt => ttt.Order)
                                .Select(ttt => new
                                {
                                    title = ttt.Title,
                                    link = ttt.Link,
                                    childs = ttt.Childs.Where(tttt => tttt.IsActive == true).OrderBy(tttt => tttt.Order).Select(tttt => new
                                    {
                                        title = tttt.Title,
                                        link = tttt.Link,
                                    }).ToList()
                                })
                                .ToList()
                        })
                        .ToList()

                })
                .ToList();
        }
    }
}
