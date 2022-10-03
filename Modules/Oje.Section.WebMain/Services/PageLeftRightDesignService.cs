using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.WebMain.Services
{
    public class PageLeftRightDesignService : IPageLeftRightDesignService
    {
        readonly WebMainDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public PageLeftRightDesignService
            (
                WebMainDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(PageLeftRightDesignCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new PageLeftRightDesign()
            {
                PageId = input.pId.ToLongReturnZiro(),
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageLeftRightDesignCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.pId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Page);
            if (!db.Pages.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.pId))
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.PageLeftRightDesigns
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;

            try { db.SaveChanges(); } catch { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public PageLeftRightDesignCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.PageLeftRightDesigns
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new PageLeftRightDesignCreateUpdateVM
                {
                    id = t.Id,
                    pId = t.PageId,
                    pId_Title = t.Page.Title,
                    title = t.Title,
                    description = t.Description,
                    order = t.Order,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<PageLeftRightDesignMainGridResultVM> GetList(PageLeftRightDesignMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new PageLeftRightDesignMainGrid();

            var qureResult = db.PageLeftRightDesigns
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.pageTitle))
                qureResult = qureResult.Where(t => t.Page.Title.Contains(searchInput.pageTitle));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<PageLeftRightDesignMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    pTitle = t.Page.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new PageLeftRightDesignMainGridResultVM
                {
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    pageTitle = t.pTitle,
                    row = ++row,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(PageLeftRightDesignCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.PageLeftRightDesigns
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.PageId = input.pId.ToLongReturnZiro();
            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.PageLeftRightDesigns.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Page.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Page.Title + "(" + t.Id + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public List<IPageWebItemVM> GetListForWeb(long? pageId, int? siteSettingId)
        {
            return db.PageLeftRightDesigns
                .Where(t => t.SiteSettingId == siteSettingId && t.PageId == pageId && t.IsActive == true)
                .Select(t => new PageLeftRightDesignWebVM
                {
                    description = t.Description,
                    order = t.Order,
                    title = t.Title,
                    type = PageWebItemType.LeftAndRight,
                    PageLeftRightDesignWebItemVMs = 
                          t.PageLeftRightDesignItems
                            .Where(tt => tt.IsActive == true)
                            .OrderBy(tt => tt.Order)
                            .Select(tt => new PageLeftRightDesignWebItemVM 
                            { 
                                description = tt.Description,
                                order = tt.Order,
                                title = tt.Title,
                                image = GlobalConfig.FileAccessHandlerUrl + tt.MainImage,
                                bLink = tt.ButtonLink,
                                bTitle = tt.ButtonTitle
                            })
                            .ToList()
                })
                .ToList()
                .OfType<IPageWebItemVM>()
                .ToList();
        }
    }
}
