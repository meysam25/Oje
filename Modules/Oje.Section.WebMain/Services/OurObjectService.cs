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
    public class OurObjectService : IOurObjectService
    {
        readonly WebMainDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public OurObjectService
            (
                WebMainDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(OurCustomerCreateUpdateVM input, int? siteSettingId, OurObjectType type)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new OurObject();

            newItem.Title = input.title;
            newItem.Subtitle = input.subtitle;
            newItem.Url = input.url;
            newItem.Type = type;
            newItem.IsActive = input.isActive.ToBooleanReturnFalse();
            newItem.SiteSettingId = siteSettingId.Value;
            newItem.ImageUrl = " ";

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainImage != null && input.mainImage.Length > 0)
            {
                newItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.OurObject, input.mainImage, null, siteSettingId, newItem.Id, ".jpg,.jpeg,.png", false);
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(OurCustomerCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            //if (string.IsNullOrEmpty(input.title))
            //    throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (!string.IsNullOrEmpty(input.title) && input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.id.ToIntReturnZiro() <= 0 && (input.mainImage == null || input.mainImage.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image);
            if (input.mainImage != null && input.mainImage.Length > 0 && !UploadedFileService.IsValidImageSize(input.mainImage, true, 0.99m, 1.01m))
                throw BException.GenerateNewException(BMessages.Invalid_Image_Size);
        }

        public ApiResult Delete(int? id, int? siteSettingId, OurObjectType type)
        {
            var foundItem = db.OurObjects.Where(t => t.SiteSettingId == siteSettingId && t.Type == type && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId, OurObjectType type)
        {
            return db.OurObjects
                .Where(t => t.SiteSettingId == siteSettingId && t.Type == type && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    subtitle = t.Subtitle,
                    isActive = t.IsActive,
                    url = t.Url,
                    mainImage_address = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl
                })
                .FirstOrDefault();
        }

        public GridResultVM<OurCustomerMainGridResultVM> GetList(OurCustomerMainGrid searchInput, int? siteSettingId, OurObjectType type)
        {
            if (searchInput == null)
                searchInput = new OurCustomerMainGrid();

            var qureResult = db.OurObjects.Where(t => t.SiteSettingId == siteSettingId && t.Type == type);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<OurCustomerMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    IsActive = t.IsActive
                })
                .ToList()
                .Select(t => new OurCustomerMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(OurCustomerCreateUpdateVM input, int? siteSettingId, OurObjectType type)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.OurObjects.Where(t => t.SiteSettingId == siteSettingId && t.Type == type && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Subtitle = input.subtitle;
            foundItem.Url = input.url;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();


            if (input.mainImage != null && input.mainImage.Length > 0)
                foundItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.OurObject, input.mainImage, null, siteSettingId, foundItem.Id, ".jpg,.jpeg,.png", false);

            db.SaveChanges();


            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListWeb(int? siteSettingId, OurObjectType type)
        {
            return db.OurObjects.Where(t => t.SiteSettingId == siteSettingId && t.Type == type).Select(t => new
            {
                title = t.Title,
                subTitle = t.Subtitle,
                img = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl,
                url = t.Url
            }).ToList();
        }
    }
}
