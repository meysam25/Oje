using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Models.View;
using Oje.Section.Tender.Services.EContext;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFileService : ITenderFileService
    {
        const string validFileExtensions = ".pdf,.png,.jpg,.jpeg";
        readonly IUploadedFileService UploadedFileService = null;
        readonly TenderDBContext db = null;
        public TenderFileService
            (
                IUploadedFileService UploadedFileService,
                TenderDBContext db
            )
        {
            this.UploadedFileService = UploadedFileService;
            this.db = db;
        }

        public ApiResult Create(TenderFileCreateUpdateVM input, int? siteSettingId)
        {
            createValidation(input, siteSettingId);

            var newItem = new TenderFile()
            {
                Title = input.title,
                FileUrl = " ",
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
                newItem.FileUrl = UploadedFileService.UploadNewFile(Infrastructure.Enums.FileType.OnlineFile, input.mainFile, null, siteSettingId, newItem.Id, validFileExtensions, false, null, input.title);
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(TenderFileCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.id.ToIntReturnZiro() <= 0 && (input.mainFile == null || input.mainFile.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(validFileExtensions))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TenderFiles.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.TenderFiles
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    mainFile_address = GlobalConfig.FileAccessHandlerUrl + t.FileUrl,
                    isActive = t.IsActive
                }).FirstOrDefault();
        }

        public GridResultVM<TenderFileMainGridResultVM> GetList(TenderFileMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TenderFileMainGrid();

            var quiryResult = db.TenderFiles.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<TenderFileMainGridResultVM>() {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                })
                .ToList()
                .Select(t => new TenderFileMainGridResultVM 
                { 
                    id = t.id,
                    row = ++row,
                    title = t.title,
                    isActive = t.isActive ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(TenderFileCreateUpdateVM input, int? siteSettingId)
        {
            createValidation(input, siteSettingId);

            var foundItem = db.TenderFiles.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;

            if (input.mainFile != null && input.mainFile.Length > 0)
                foundItem.FileUrl = UploadedFileService.UploadNewFile(Infrastructure.Enums.FileType.OnlineFile, input.mainFile, null, siteSettingId, foundItem.Id, validFileExtensions, false, null, input.title);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListForWeb(int? siteSettingId)
        {
            return db.TenderFiles
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    src = GlobalConfig.FileAccessHandlerUrl + t.FileUrl
                })
                .ToList();
        }
    }
}
