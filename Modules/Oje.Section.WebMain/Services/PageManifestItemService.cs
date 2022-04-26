using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.WebMain.Services
{
    public class PageManifestItemService : IPageManifestItemService
    {
        readonly WebMainDBContext db = null;
        public PageManifestItemService
            (
                WebMainDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(PageManifestItemCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new PageManifestItem()
            {
                PageManifestId = input.mid.Value,
                Title = input.title,
                Description = input.description,
                Order = input.order.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageManifestItemCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.mid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (!db.PageManifests.Any(t => t.Id == input.mid && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 200)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_200_chars);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.PageManifestItems.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public PageManifestItemCreateUpdateVM GetById(long? id, int? siteSettingId)
        {
            return db.PageManifestItems.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => new PageManifestItemCreateUpdateVM
            {
                id = t.Id,
                mid = t.PageManifestId,
                mid_Title = t.PageManifest.Title,
                title = t.Title,
                description = t.Description,
                order = t.Order,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<PageManifestItemMainGridResultVM> GetList(PageManifestItemMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new PageManifestItemMainGrid();

            var quiryResult = db.PageManifestItems.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.midTitle))
                quiryResult = quiryResult.Where(t => t.PageManifest.Title.Contains(searchInput.midTitle));
            if (!string.IsNullOrEmpty(searchInput.pageTitle))
                quiryResult = quiryResult.Where(t => t.PageManifest.Page.Title.Contains(searchInput.pageTitle));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<PageManifestItemMainGridResultVM>()
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
                    midTitle = t.PageManifest.Title,
                    pageTitle = t.PageManifest.Page.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new PageManifestItemMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    title = t.title,
                    midTitle = t.midTitle,
                    pageTitle = t.pageTitle
                })
                .ToList()
            };
        }

        public ApiResult Update(PageManifestItemCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.PageManifestItems.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.PageManifestId = input.mid.Value;
            foundItem.Title = input.title;
            foundItem.Description = input.description;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
