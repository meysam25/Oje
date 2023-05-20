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
    public class SecretariatUserDigitalSignatureService : ISecretariatUserDigitalSignatureService
    {
        readonly static string validExtensions = ".png";
        readonly SecretariatDBContext db = null;
        readonly IUserService UserService = null;
        readonly IUploadedFileService UploadedFileService = null;
        public SecretariatUserDigitalSignatureService
            (
                SecretariatDBContext db,
                IUserService UserService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(SecretariatUserDigitalSignatureCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new SecretariatUserDigitalSignature()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                Role = input.role,
                SignatureSamle = " ",
                SiteSettingId = siteSettingId.Value,
                UserId = input.userId.Value,
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.signature != null && input.signature.Length > 0)
                newItem.SignatureSamle = 
                    UploadedFileService.UploadNewFile(FileType.SecretariatUserDigitalSignature, input.signature, null, siteSettingId, newItem.Id, validExtensions, true, null, input.role);

            db.SaveChanges();
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SecretariatUserDigitalSignatureCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.role))
                throw BException.GenerateNewException(BMessages.Please_Enter_Role);
            if (input.role.Length > 100)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.id.ToIntReturnZiro() <= 0 && (input.signature == null || input.signature.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_File);
            if (input.signature != null && input.signature.Length > 0 && !input.signature.IsValidExtension(validExtensions))
                throw BException.GenerateNewException(BMessages.Invalid_File);
            if (input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (!string.IsNullOrEmpty(input.title) && input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!UserService.Exist(input.userId, siteSettingId))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
            if (db.SecretariatUserDigitalSignatures.Any(t => t.SiteSettingId == siteSettingId && t.Id != input.id && t.UserId == input.userId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.SecretariatUserDigitalSignatures.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            try
            {
                db.Entry(foundItem).State = EntityState.Deleted;
                db.SaveChanges();
            } catch (Exception) { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }
            
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SecretariatUserDigitalSignatures
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new 
                {
                    id = t.Id,
                    role = t.Role,
                    userId = t.UserId,
                    userId_Title = t.User.Firstname + " " + t.User.Lastname,
                    isActive = t.IsActive,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<SecretariatUserDigitalSignatureMainGridResultVM> GetList(SecretariatUserDigitalSignatureMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatUserDigitalSignatures.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.role))
                quiryResult = quiryResult.Where(t => t.Role.Contains(searchInput.role));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => (t.User.Username + " " + t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));

            int row = searchInput.skip;

            return new GridResultVM<SecretariatUserDigitalSignatureMainGridResultVM>() 
            {
                total = quiryResult.Count(), 
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    role = t.Role,
                    t.IsActive,
                    user = t.User.Username + "(" + t.User.Firstname + " " + t.User.Lastname + ")"
                })
                .ToList()
                .Select(t => new SecretariatUserDigitalSignatureMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    role = t.role,
                    user = t.user
                })
                .ToList()
            };
        }

        public ApiResult Update(SecretariatUserDigitalSignatureCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.SecretariatUserDigitalSignatures.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Role = input.role;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.UserId = input.userId.Value;
            foundItem.Title = input.title;
            if (input.signature != null && input.signature.Length > 0)
                foundItem.SignatureSamle =
                    UploadedFileService.UploadNewFile(FileType.SecretariatUserDigitalSignature, input.signature, null, siteSettingId, foundItem.Id, validExtensions, true, null, input.role);

            db.SaveChanges();
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.SecretariatUserDigitalSignatures.Any(t => t.SiteSettingId == siteSettingId && t.Id == id && t.IsActive == true);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                (
                    db.SecretariatUserDigitalSignatures
                    .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                    .Select(t => new 
                    {
                        id = t.Id,
                        title = t.User.Firstname + " " + t.User.Lastname
                    })
                    .ToList()
                );

            return result;
        }
    }
}
