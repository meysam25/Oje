using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
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
    public class SecretariatLetterReciveService : ISecretariatLetterReciveService
    {
        readonly SecretariatDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public SecretariatLetterReciveService
            (
                SecretariatDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(SecretariatLetterReciveCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            db.Entry(new SecretariatLetterRecive()
            {
                CreateDate = DateTime.Now,
                Date = input.date.ToEnDate().Value,
                Number = input.number,
                Mobile = input.mobile,
                SiteSettingId = siteSettingId.Value,
                UserId = loginUserId.Value,
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatLetterReciveCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.date))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.date.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (!string.IsNullOrEmpty(input.mobile) && !input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (input.number.Length > 50)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatLetterRecives
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.SecretariatLetterRecives
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    date = t.Date,
                    number = t.Number,
                    mobile = t.Mobile,
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .Select(t => new
                {
                    t.id,
                    t.number,
                    t.mobile,
                    date = t.date.ToFaDate()
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatLetterReciveMainGridResultVM> GetList(SecretariatLetterReciveMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatLetterRecives.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.mobile))
                quiryResult = quiryResult.Where(t => t.Mobile == searchInput.mobile);
            if (!string.IsNullOrEmpty(searchInput.number))
                quiryResult = quiryResult.Where(t => t.Number.Contains(searchInput.number));
            if (!string.IsNullOrEmpty(searchInput.date) && searchInput.date.ToEnDate() != null)
            {
                var targetDate = searchInput.date.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.Date.Year == targetDate.Year && t.Date.Month == targetDate.Month && t.Date.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));

            int row = searchInput.skip;
            return new GridResultVM<SecretariatLetterReciveMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    date = t.Date,
                    number = t.Number,
                    mobile = t.Mobile,
                    createDate = t.CreateDate,
                    user = t.User.Firstname + " " + t.User.Lastname
                })
                .ToList()
                .Select(t => new SecretariatLetterReciveMainGridResultVM()
                {
                    row = ++row,
                    id = t.id,
                    createDate = t.createDate.ToFaDate(),
                    date = t.date.ToFaDate(),
                    mobile = t.mobile,
                    number = t.number,
                    user = t.user
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatLetterReciveCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            var foundItem = db.SecretariatLetterRecives
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Number = input.number;
            foundItem.Date = input.date.ToEnDate().Value;
            foundItem.Mobile = input.mobile;
            foundItem.UserId = loginUserId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object DeleteUploadFile(long? fileId, long? id, int? siteSettingId)
        {
            var foundItem =
                db.SecretariatLetters
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.Delete(fileId, siteSettingId, foundItem.Id, FileType.SecretariatReciveAttachments);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetUploadFiles(GlobalGridParentLong input, int? siteSettingId)
        {
            input = input ?? new GlobalGridParentLong();

            var quiryResult = db.SecretariatLetterRecives
                .Where(t => t.Id == input.pKey && t.SiteSettingId == siteSettingId);

            var foundItemId =
                quiryResult
                .Select(t => t.Id)
                .FirstOrDefault();

            if (foundItemId <= 0)
                foundItemId = -1;

            List<FileType> fStatus = new() { FileType.SecretariatReciveAttachments };

            return new
            {
                total = UploadedFileService.GetCountBy(foundItemId, fStatus, siteSettingId),
                data = UploadedFileService.GetListBy(foundItemId, fStatus, input.skip, input.take, siteSettingId)
            };
        }

        public ApiResult UploadNewFile(long? id, IFormFile mainFile, string title, int? siteSettingId, long? loginUserId)
        {
            const string validFileExtension = ".jpg,.png,.jpeg";
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
                db.SecretariatLetterRecives
                .Where(t => t.Id == id)
                .Select(t => t.Id)
                .FirstOrDefault();
            if (foundItemId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            UploadedFileService.UploadNewFile(FileType.SecretariatReciveAttachments, mainFile, loginUserId, siteSettingId, foundItemId, validFileExtension, true, null, title);


            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
