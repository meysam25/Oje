using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.DB;
using Oje.Section.Secretariat.Models.View;
using Oje.Section.Secretariat.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Secretariat.Services
{
    public class SecretariatLetterService : ISecretariatLetterService
    {
        readonly SecretariatDBContext db = null;
        readonly ISecretariatLetterCategoryService SecretariatLetterCategoryService = null;
        readonly ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService = null;
        readonly ISecretariatLetterUserService SecretariatLetterUserService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public SecretariatLetterService
            (
                SecretariatDBContext db,
                ISecretariatLetterCategoryService SecretariatLetterCategoryService,
                ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService,
                ISecretariatLetterUserService SecretariatLetterUserService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService
            )
        {
            this.db = db;
            this.SecretariatLetterCategoryService = SecretariatLetterCategoryService;
            this.SecretariatUserDigitalSignatureService = SecretariatUserDigitalSignatureService;
            this.SecretariatLetterUserService = SecretariatLetterUserService;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
        }

        public ApiResult Create(SecretariatLetterCreateUpdateVM input, int? siteSettingId, long? userId)
        {
            createUpdateValidation(input, siteSettingId, userId);

            var newItem = new SecretariatLetter()
            {
                Body = input.description,
                CreateDate = DateTime.Now,
                SecretariatLetterCategoryId = input.cId.Value,
                SecretariatUserDigitalSignatureId = input.cId.Value,
                SiteSettingId = siteSettingId.Value,
                UserId = userId.Value,
                Subject = input.subject,
                SubTitle = input.subTitle,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            SecretariatLetterUserService.Create(newItem.Id, input.mobile, newItem.Title, siteSettingId.Value, userId.Value, SecretariatLetterUserType.Owner);
            UserNotifierService.Notify(userId, UserNotificationType.ConfirmSecretariatLetter, null, newItem.Id, newItem.Title, siteSettingId, "/Account/Dashboard/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatLetterCreateUpdateVM input, int? siteSettingId, long? userId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 200)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_200_chars);
            if (string.IsNullOrEmpty(input.subTitle))
                throw BException.GenerateNewException(BMessages.Please_Enter_SubTitle);
            if (input.subTitle.Length > 200)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(input.subject))
                throw BException.GenerateNewException(BMessages.Please_Enter_Subject);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.cId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Category);
            if (!SecretariatLetterCategoryService.Exist(siteSettingId, input.cId))
                throw BException.GenerateNewException(BMessages.Please_Select_Category);
            if (input.sId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Signer);
            if (!SecretariatUserDigitalSignatureService.Exist(siteSettingId, input.sId))
                throw BException.GenerateNewException(BMessages.Please_Select_Signer);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);

        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatLetters.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            try
            {
                db.Entry(foundItem).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception) { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.SecretariatLetters
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new SecretariatLetterCreateUpdateVM
                {
                    id = t.Id,
                    cId = t.SecretariatLetterCategoryId,
                    description = t.Body,
                    sId = t.SecretariatUserDigitalSignatureId,
                    subject = t.Subject,
                    subTitle = t.SubTitle,
                    title = t.Title,
                    mobile = t.SecretariatLetterUsers
                                    .Where(tt => tt.Type == Infrastructure.Enums.SecretariatLetterUserType.Owner)
                                    .Select(tt => tt.User.Username)
                                    .FirstOrDefault()
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatLetterMainGridResult> GetList(SecretariatLetterMainGrid searchInput, int? siteSettingId, long? loginUserId = null)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatLetters.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.subTitle))
                quiryResult = quiryResult.Where(t => t.SubTitle.Contains(searchInput.subTitle));
            if (!string.IsNullOrEmpty(searchInput.subject))
                quiryResult = quiryResult.Where(t => t.Subject.Contains(searchInput.subject));
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));
            if (!string.IsNullOrEmpty(searchInput.sUser))
                quiryResult = quiryResult.Where(t => (t.SecretariatUserDigitalSignature.User.Firstname + " " + t.SecretariatUserDigitalSignature.User.Lastname).Contains(searchInput.sUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            if (loginUserId.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.SecretariatLetterUsers.Any(t => t.UserId == loginUserId) && t.IsConfirm == true);

            int row = searchInput.skip;

            return new GridResultVM<SecretariatLetterMainGridResult>()
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
                    subTitle = t.SubTitle,
                    subject = t.Subject,
                    user = t.User.Firstname + " " + t.User.Lastname,
                    sUser = t.SecretariatUserDigitalSignature.User.Firstname + " " + t.SecretariatUserDigitalSignature.User.Lastname,
                    t.CreateDate,
                    t.IsConfirm
                })
                .ToList()
                .Select(t => new SecretariatLetterMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    subTitle = t.subTitle,
                    subject = t.subject,
                    sUser = t.sUser,
                    user = t.user,
                    createDate = t.CreateDate.ToFaDate(),
                    isC = t.IsConfirm
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatLetterCreateUpdateVM input, int? siteSettingId, long? userId)
        {
            createUpdateValidation(input, siteSettingId, userId);

            var foundItem = db.SecretariatLetters.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Body = input.description;
            foundItem.SecretariatLetterCategoryId = input.cId.Value;
            foundItem.SecretariatUserDigitalSignatureId = input.cId.Value;
            foundItem.Subject = input.subject;
            foundItem.SubTitle = input.subTitle;
            foundItem.Title = input.title;
            foundItem.IsConfirm = false;

            db.SaveChanges();

            SecretariatLetterUserService.Update(foundItem.Id, input.mobile, foundItem.Title, siteSettingId.Value, userId.Value, SecretariatLetterUserType.Owner);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Confirm(long? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatLetters
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Include(t => t.SecretariatLetterUsers)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.SecretariatLetterUsers == null || !foundItem.SecretariatLetterUsers.Any(t => t.Type == SecretariatLetterUserType.Owner))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            foundItem.IsConfirm = true;
            db.SaveChanges();

            var userId = foundItem.SecretariatLetterUsers.Where(t => t.Type == SecretariatLetterUserType.Owner).Select(t => t.UserId).FirstOrDefault();

            UserNotifierService.Notify(userId, UserNotificationType.ConfirmSecretariatLetter, null, foundItem.Id, foundItem.Title, siteSettingId, "/Secretariat/MySecretariatLetter/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public SecretariatLetterVM PdfDetailes(long id, int? siteSettingId, long? loginUserId)
        {
            var quiryResult = db.SecretariatLetters
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId);

            if (loginUserId.ToLongReturnZiro() > 0)
                quiryResult = quiryResult
                        .Where(t => t.IsConfirm == true && t.SecretariatLetterUsers.Any(tt => tt.UserId == loginUserId));

            return
                quiryResult
                .Select(t => new SecretariatLetterVM
                {
                    header = GlobalConfig.FileAccessHandlerUrl + t.SecretariatLetterCategory.SecretariatHeaderFooter.HeaderUrl,
                    footer = GlobalConfig.FileAccessHandlerUrl + t.SecretariatLetterCategory.SecretariatHeaderFooter.FooterUrl,
                    headerDescription = t.SecretariatLetterCategory.SecretariatHeaderFooterDescription.Header,
                    footerDescription = t.SecretariatLetterCategory.SecretariatHeaderFooterDescription.Footer,
                    hasLink = db.UploadedFiles.Any(tt => tt.ObjectId == t.Id && tt.FileType == FileType.SecretariatAttachments),
                    createDate = t.CreateDate,
                    number = t.SecretariatLetterCategory.Code + "/" + t.Id,
                    title = t.Title,
                    subTitle = t.SubTitle,
                    subject = t.Subject,
                    sUserTitle = t.SecretariatUserDigitalSignature.Title,
                    sUserRole = t.SecretariatUserDigitalSignature.Role,
                    sUserSignature = GlobalConfig.FileAccessHandlerUrl + t.SecretariatUserDigitalSignature.SignatureSamle,
                    sUserFirstname = t.SecretariatUserDigitalSignature.User.Firstname,
                    sUserLastname = t.SecretariatUserDigitalSignature.User.Lastname,
                    description = t.Body
                })
                .FirstOrDefault();
        }

        public object DeleteUploadFile(long? fileId, long? pKey, int? siteSettingId)
        {
            var foundItem =
                db.SecretariatLetters
                .Where(t => t.Id == pKey && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.Delete(fileId, siteSettingId, foundItem.Id, FileType.SecretariatAttachments);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetUploadFiles(GlobalGridParentLong input, int? siteSettingId, long? loginUserId)
        {
            input = input ?? new GlobalGridParentLong();

            var quiryResult = db.SecretariatLetters
                .Where(t => t.Id == input.pKey && t.SiteSettingId == siteSettingId);

            if (loginUserId.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.SecretariatLetterUsers.Any(t => t.UserId == loginUserId && t.SecretariatLetter.IsConfirm == true));

            var foundItemId =
                quiryResult
                .Select(t => t.Id)
                .FirstOrDefault();

            if (foundItemId <= 0)
                foundItemId = -1;

            List<FileType> fStatus = new() { FileType.SecretariatAttachments };

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, fStatus, siteSettingId),
                data = UploadedFileService.GetListBy(foundItemId, fStatus, input.skip, input.take, siteSettingId)
            };
        }

        public object UploadNewFile(long? pKey, IFormFile mainFile, string title, int? siteSettingId, long? loginUserId)
        {
            const string validFileExtension = ".jpg,.png,.jpeg,.pdf,.doc,.docx,.zip";
            if (mainFile == null)
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (!mainFile.IsValidExtension(validFileExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
            if (string.IsNullOrEmpty(title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (!string.IsNullOrEmpty(title) && title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Active_By_Admin_First);

            var foundItemId =
                db.SecretariatLetters
                .Where(t => t.Id == pKey)
                .Select(t => t.Id)
                .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.UploadNewFile(FileType.SecretariatAttachments, mainFile, loginUserId, siteSettingId, foundItemId, validFileExtension, true, null, title);


            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
