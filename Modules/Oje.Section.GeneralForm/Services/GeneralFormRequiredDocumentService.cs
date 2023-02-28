using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormRequiredDocumentService : IGeneralFormRequiredDocumentService
    {
        readonly GeneralFormDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;

        static string acceptExtension = ".jpg,.jpeg,.png";

        public GeneralFormRequiredDocumentService
            (
                GeneralFormDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(GeneralFormRequiredDocumentCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var newItem = new GeneralFormRequiredDocument()
            {
                GeneralFormId = input.fId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Name = input.name,
                Title = input.title,
                SampleUrl = " ",
                IsRequired = input.isRequired
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
                newItem.SampleUrl = UploadedFileService.UploadNewFile(FileType.GeneralForm, input.mainFile, null, null, newItem.Id, acceptExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(GeneralFormRequiredDocumentCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (input.name.Length > 100)
                throw BException.GenerateNewException(BMessages.Name_Can_Not_Be_More_Then_100_chars);
            if (input.id.ToIntReturnZiro() <= 0 && (input.mainFile == null || input.mainFile.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.fId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(acceptExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
        }

        public ApiResult Delete(long? id)
        {
            var foundItem = db.GeneralFormRequiredDocuments.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id)
        {
            return db.GeneralFormRequiredDocuments
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    fId = t.GeneralFormId,
                    fId_Title = t.GeneralForm.Title,
                    isActive = t.IsActive,
                    name = t.Name,
                    title = t.Title,
                    mainFile_address = !string.IsNullOrEmpty(t.SampleUrl) && t.SampleUrl != " " ? GlobalConfig.FileAccessHandlerUrl + t.SampleUrl : "",
                    isRequired = t.IsRequired == null ? false : t.IsRequired.Value
                })
                .FirstOrDefault();
        }

        public GridResultVM<GeneralFormRequiredDocumentMainGridResult> GetList(GeneralFormRequiredDocumentMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GeneralFormRequiredDocuments.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.fId))
                quiryResult = quiryResult.Where(t => t.GeneralForm.Title.Contains(searchInput.fId));
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.name))
                quiryResult = quiryResult.Where(t => t.Name.Contains(searchInput.name));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<GeneralFormRequiredDocumentMainGridResult>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderBy(t => t.GeneralFormId).ThenByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    fId = t.GeneralForm.Title,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new GeneralFormRequiredDocumentMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    fId = t.fId,
                    title = t.title,
                    name = t.name,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(GeneralFormRequiredDocumentCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.GeneralFormRequiredDocuments.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.GeneralFormId = input.fId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Name = input.name;
            foundItem.Title = input.title;
            foundItem.IsRequired = input.isRequired;

            if (input.mainFile != null && input.mainFile.Length > 0)
                foundItem.SampleUrl = UploadedFileService.UploadNewFile(FileType.GeneralForm, input.mainFile, null, null, foundItem.Id, acceptExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<GeneralFormRequiredDocument> GetActiveListNoTracking(long generalFormId)
        {
            return db.GeneralFormRequiredDocuments.Where(t => t.GeneralFormId == generalFormId).AsNoTracking().ToList();
        }

        public List<GeneralFormRequiredDocumentVM> GetActiveForWeb(int? generalFormId, int? siteSettingId)
        {
            return db.GeneralFormRequiredDocuments
                .Where(t => t.GeneralFormId == generalFormId && t.IsActive == true && (t.GeneralForm.SiteSettingId == null || t.GeneralForm.SiteSettingId == siteSettingId))
                .Select(t => new GeneralFormRequiredDocumentVM 
                {
                    title = t.Title,
                    name = t.Name,
                    isRequired = t.IsRequired == null ? false : t.IsRequired.Value,
                    sample = GlobalConfig.FileAccessHandlerUrl + t.SampleUrl
                })
                .ToList();
        }
    }
}
