using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class PageManifestService : IPageManifestService
    {
        readonly WebMainDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public PageManifestService
            (
                WebMainDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(PageManifestCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new PageManifest()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                PageId = input.pid.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageManifestCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.pid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Page);
            if (!db.Pages.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.pid))
                throw BException.GenerateNewException(BMessages.Please_Select_Page);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.PageManifests
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.PageManifests
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    order = t.Order,
                    pid = t.PageId,
                    pid_Title = t.Page.Title,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<PageManifestMainGridResultVM> GetList(PageManifestMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new PageManifestMainGrid();

            var quiryResult = db.PageManifests
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.pageTitle))
                quiryResult = quiryResult.Where(t => t.Page.Title.Contains(searchInput.pageTitle));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<PageManifestMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    pageTitle = t.Page.Title,
                    isActive = t.IsActive,
                    order = t.Order,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new PageManifestMainGridResultVM()
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    pageTitle = t.pageTitle,
                    title = t.title,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(PageManifestCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.PageManifests
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.PageId = input.pid.Value;
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

            var qureResult = db.PageManifests.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Page.Title + "(" + t.Title + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = (t.Page.Title + "(" + t.Title + ")") }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public IEnumerable<IPageWebItemVM> GetListForWeb(long? pageId, int? siteSettingId)
        {
            return db.PageManifests
                .Where(t => t.SiteSettingId == siteSettingId && t.PageId == pageId && t.IsActive == true)
                .Select(t => new PageLeftRightDesignWebVM
                {
                    order = t.Order,
                    title = t.Title,
                    type = PageWebItemType.Manifest,
                    PageLeftRightDesignWebItemVMs =
                          t.PageManifestItems
                            .Where(tt => tt.IsActive == true)
                            .OrderBy(tt => tt.Order)
                            .Select(tt => new PageLeftRightDesignWebItemVM
                            {
                                description = tt.Description,
                                order = tt.Order,
                                title = tt.Title
                            })
                            .ToList()
                })
                .ToList()
                .OfType<IPageWebItemVM>()
                .ToList();
        }
    }
}
