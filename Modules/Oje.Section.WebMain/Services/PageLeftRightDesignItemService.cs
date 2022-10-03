using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
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
    public class PageLeftRightDesignItemService : IPageLeftRightDesignItemService
    {
        readonly WebMainDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public PageLeftRightDesignItemService
            (
                WebMainDBContext db,
                IUploadedFileService UploadedFileService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new PageLeftRightDesignItem()
            {
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                PageLeftRightDesignId = input.dId.ToLongReturnZiro(),
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                MainImage = " ",
                ButtonTitle = input.bTitle,
                ButtonLink = input.bLink
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainImage != null && input.mainImage.Length > 0)
                newItem.MainImage = UploadedFileService.UploadNewFile(FileType.PageLeftRightDesignItem, input.mainImage, null, siteSettingId, newItem.Id, ".jpg,.png,.jpeg", false, null);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Operation_Was_Successfull);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.dId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Design);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 200)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_200_chars);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.id.ToLongReturnZiro() <= 0 && (input.mainImage == null || input.mainImage.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image);
            if (!db.PageLeftRightDesigns.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.dId))
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.PageLeftRightDesignItems
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public PageLeftRightDesignItemCreateUpdateVM GetById(long? id, int? siteSettingId)
        {
            return db.PageLeftRightDesignItems
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new PageLeftRightDesignItemCreateUpdateVM
                {
                    id = t.Id,
                    dId = t.PageLeftRightDesignId,
                    dId_Title = t.PageLeftRightDesign.Page.Title + "(" + t.PageLeftRightDesignId + ")",
                    title = t.Title,
                    description = t.Description,
                    order = t.Order,
                    isActive = t.IsActive,
                    mainImage_address = GlobalConfig.FileAccessHandlerUrl + t.MainImage,
                    bTitle = t.ButtonTitle,
                    bLink = t.ButtonLink
                })
                .FirstOrDefault();
        }

        public GridResultVM<PageLeftRightDesignItemMainGridResultVM> GetList(PageLeftRightDesignItemMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new PageLeftRightDesignItemMainGrid();

            var qureResult = db.PageLeftRightDesignItems
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.pageTitle))
                qureResult = qureResult.Where(t => t.PageLeftRightDesign.Page.Title.Contains(searchInput.pageTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<PageLeftRightDesignItemMainGridResultVM>()
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
                    pageTitle = t.PageLeftRightDesign.Page.Title,
                    t.PageLeftRightDesignId,
                    t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new PageLeftRightDesignItemMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    pageTitle = t.pageTitle + "(" + t.PageLeftRightDesignId + ")",
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.PageLeftRightDesignItems
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.PageLeftRightDesignId = input.dId.ToLongReturnZiro();
            foundItem.Title = input.title;
            foundItem.ButtonLink = input.bLink;
            foundItem.ButtonTitle = input.bTitle;


            if (input.mainImage != null && input.mainImage.Length > 0)
                foundItem.MainImage = UploadedFileService.UploadNewFile(FileType.PageLeftRightDesignItem, input.mainImage, null, siteSettingId, foundItem.Id, ".jpg,.png,.jpeg", false, null);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
