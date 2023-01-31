using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserRegisterFormRequiredDocumentService : IUserRegisterFormRequiredDocumentService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserRegisterFormRequiredDocumentService(
                RegisterFormDBContext db,
                IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService,
                IUploadedFileService UploadedFileService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserRegisterFormRequiredDocumentTypeService = UserRegisterFormRequiredDocumentTypeService;
            this.UploadedFileService = UploadedFileService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormRequiredDocumentCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var newItem = new UserRegisterFormRequiredDocument()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsRequired = input.isRequird.ToBooleanReturnFalse(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                UserRegisterFormRequiredDocumentTypeId = input.typeId.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.downloadFile != null && input.downloadFile.Length > 0)
            {
                newItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.RegisterDownloadSampleDocuments, input.downloadFile, null, siteSettingId, newItem.Id, ".jpg,.png,.doc,.docx,.pdf,.jpeg", false);
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormRequiredDocumentCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.typeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (UserRegisterFormRequiredDocumentTypeService.GetById(input.typeId, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId) == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormRequiredDocuments
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterFormRequiredDocuments
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    typeId = t.UserRegisterFormRequiredDocumentTypeId,
                    isRequird = t.IsRequired,
                    isActive = t.IsActive,
                    downloadFile_address = !string.IsNullOrEmpty(t.DownloadFile) ? GlobalConfig.FileAccessHandlerUrl + t.DownloadFile : "",
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormRequiredDocumentMainGridResultVM> GetList(UserRegisterFormRequiredDocumentMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserRegisterFormRequiredDocumentMainGrid();

            var qureResult = db.UserRegisterFormRequiredDocuments.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.typeTitle))
                qureResult = qureResult.Where(t => t.UserRegisterFormRequiredDocumentType.Title.Contains(searchInput.typeTitle));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isRequired != null)
                qureResult = qureResult.Where(t => t.IsRequired == searchInput.isRequired);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormRequiredDocumentMainGridResultVM>()
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
                        isActive = t.IsActive,
                        t.IsRequired,
                        typeTitle = t.UserRegisterFormRequiredDocumentType.Title,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new UserRegisterFormRequiredDocumentMainGridResultVM
                    {
                        id = t.id,
                        row = ++row,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        isRequired = t.IsRequired == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                        title = t.title,
                        typeTitle = t.typeTitle,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormRequiredDocumentCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.UserRegisterFormRequiredDocuments
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsRequired = input.isRequird.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.UserRegisterFormRequiredDocumentTypeId = input.typeId.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            if (input.downloadFile != null && input.downloadFile.Length > 0)
                foundItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.RegisterDownloadSampleDocuments, input.downloadFile, null, siteSettingId, foundItem.Id, ".jpg,.png,.doc,.docx,.pdf,.jpeg", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId, int? formId)
        {
            return db.UserRegisterFormRequiredDocuments
                .Where(t =>
                        t.UserRegisterFormRequiredDocumentType.UserRegisterFormId == formId && t.IsActive == true && t.UserRegisterFormRequiredDocumentType.IsActive == true && t.SiteSettingId == siteSettingId
                      )
                .Select(t => new
                {
                    title = t.Title,
                    isRequired = t.IsRequired,
                    sample = t.DownloadFile
                })
                .ToList()
                .Select(t => new
                {
                    t.title,
                    t.isRequired,
                    sample = !string.IsNullOrEmpty(t.sample) ? (GlobalConfig.FileAccessHandlerUrl + t.sample) : "",
                    name = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(t.title))
                })
                .ToList();
        }

        public List<UserRegisterFormRequiredDocument> GetRequiredDocuments(int formId, int? siteSettingId)
        {
            return db.UserRegisterFormRequiredDocuments
                .Where(t => t.UserRegisterFormRequiredDocumentType.UserRegisterFormId == formId && t.IsActive == true && t.SiteSettingId == siteSettingId && t.UserRegisterFormRequiredDocumentType.IsActive == true)
                .AsNoTracking()
                .ToList();
        }
    }
}
