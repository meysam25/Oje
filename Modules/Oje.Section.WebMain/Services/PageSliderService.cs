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
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.WebMain.Services
{
    public class PageSliderService : IPageSliderService
    {
        readonly WebMainDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        static readonly string validFileExtension = ".jpg,.png,.jpeg";

        public PageSliderService
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

        public ApiResult Create(PageSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new PageSlider()
            {
                ImageUrl = " ",
                IsActive = input.isActive.ToBooleanReturnFalse(),
                PageId = input.pid.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            newItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.PageSlider, input.mainImage, loginUserId, siteSettingId, newItem.Id, validFileExtension, false);
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageSliderCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.pid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Page);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.id.ToLongReturnZiro() <= 0 && (input.mainImage == null || input.mainImage.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.mainImage != null && input.mainImage.Length > 0 && !input.mainImage.IsValidExtension(validFileExtension))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (!db.Pages.Any(t => t.Id == input.pid && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_Page);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.PageSliders
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
            return db.PageSliders
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    pid = t.PageId,
                    pid_Title = t.Page.Title,
                    title = t.Title,
                    mainImage_address = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<PageSliderMainGridResultVM> GetList(PageSliderMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.PageSliders
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.page))
                quiryResult = quiryResult.Where(t => t.Page.Title.Contains(searchInput.page));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));


            int row = searchInput.skip;

            return new GridResultVM<PageSliderMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    page = t.Page.Title,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new PageSliderMainGridResultVM
                {

                    id = t.id,
                    row = ++row,
                    title = t.title,
                    page = t.page,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(PageSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.PageSliders
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.PageId = input.pid.Value;
            foundItem.Title = input.title;

            if (input.mainImage != null && input.mainImage.Length > 0)
                foundItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.PageSlider, input.mainImage, loginUserId, siteSettingId, foundItem.Id, validFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<PageWebSliderVM> GetLightList(long? pageId, int? siteSettingId)
        {
            return db.PageSliders
                .Where(t => t.PageId == pageId && t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new PageWebSliderVM
                {
                    title = t.Title,
                    img = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl
                })
                .ToList();
        }
    }
}
