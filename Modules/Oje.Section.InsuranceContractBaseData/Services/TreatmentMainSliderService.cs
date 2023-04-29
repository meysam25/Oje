using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class TreatmentMainSliderService: ITreatmentMainSliderService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        static string validFileExtension = ".jpg,.jpeg,.png";

        public TreatmentMainSliderService
            (
                InsuranceContractBaseDataDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(TreatmentMainSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            var newItem = new TreatmentMainSlider() 
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Link = input.link,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
                newItem.ImageSrc = UploadedFileService.UploadNewFile(FileType.TreadmentMainBanner, input.mainFile, loginUserId, siteSettingId, newItem.Id, validFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TreatmentMainSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (loginUserId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.id.ToIntReturnZiro() <= 0 && (input.mainFile == null || input.mainFile.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(validFileExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TreatmentMainSliders.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.TreatmentMainSliders
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    link = t.Link,
                    isActive = t.IsActive,
                    src = t.ImageSrc
                })
                .ToList()
                .Select(t => new 
                {
                    t.id,
                    t.title,
                    t.link,
                    t.isActive,
                    mainFile_address = GlobalConfig.FileAccessHandlerUrl + t.src
                })
                .FirstOrDefault();
        }

        public GridResultVM<TreatmentMainSliderMainGridResultVM> GetList(TreatmentMainSliderMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.TreatmentMainSliders.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<TreatmentMainSliderMainGridResultVM>()
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
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new TreatmentMainSliderMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(TreatmentMainSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            var foundItem = db.TreatmentMainSliders.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Link = input.link;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            if (input.mainFile != null && input.mainFile.Length > 0)
                foundItem.ImageSrc = UploadedFileService.UploadNewFile(FileType.TreadmentMainBanner, input.mainFile, loginUserId, siteSettingId, foundItem.Id, validFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListFormWebsite(int? siteSettingId)
        {
            return db.TreatmentMainSliders
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new 
                {
                    title = t.Title,
                    url = t.Link,
                    img = GlobalConfig.FileAccessHandlerUrl + t.ImageSrc
                })
                .ToList();
        }
    }
}
