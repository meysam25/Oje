using Microsoft.EntityFrameworkCore;
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
using System.Linq;

namespace Oje.Section.Secretariat.Services
{
    public class SecretariatLetterUserService : ISecretariatLetterUserService
    {
        readonly SecretariatDBContext db = null;
        readonly IUserService UserService = null;
        readonly IUserNotifierService UserNotifierService = null;
        public SecretariatLetterUserService
            (
                SecretariatDBContext db,
                IUserService UserService,
                IUserNotifierService UserNotifierService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.UserNotifierService = UserNotifierService;
        }

        public long? Create(long secretariatLetterId, string mobile, string fullname, int siteSettingId, long userId, SecretariatLetterUserType type)
        {
            if (secretariatLetterId > 0 && !string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(fullname) && siteSettingId > 0 && userId > 0)
            {
                var foundUserId = UserService.GetOrCreateByUsername(mobile, siteSettingId, fullname, userId);

                if (foundUserId.ToLongReturnZiro() > 0)
                {
                    if (!db.SecretariatLetterUsers.Any(t => t.SecretariatLetterId == secretariatLetterId && t.UserId == foundUserId))
                    {
                        db.Entry(new SecretariatLetterUser()
                        {
                            SecretariatLetterId = secretariatLetterId,
                            Type = type,
                            UserId = foundUserId.Value,
                            ByUserId = userId,
                            CreateDate = DateTime.Now
                        }).State = EntityState.Added;
                        db.SaveChanges();

                        return foundUserId.Value;
                    }

                }
            }
            return null;
        }

        public ApiResult CreateForWeb(SecretariatLetterUserCreateVM input, int? siteSettingId, long? userId)
        {
            var foundItem = createForWebValidation(input, siteSettingId, userId);
            var targetUserId = Create(input.pKey.Value, input.mobile, input.firstName + " " + input.lastName, siteSettingId.Value, userId.Value, SecretariatLetterUserType.Reffer);
            if (targetUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.UnknownError);

            UserNotifierService.Notify(targetUserId, UserNotificationType.ConfirmSecretariatLetter, null, input.pKey, foundItem.Title, siteSettingId, "/Secretariat/MySecretariatLetter/Index");
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private SecretariatLetter createForWebValidation(SecretariatLetterUserCreateVM input, int? siteSettingId, long? userId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.firstName))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname);
            if (string.IsNullOrEmpty(input.lastName))
                throw BException.GenerateNewException(BMessages.Please_Enter_Lastname);
            if (input.firstName.Length > 50)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.lastName.Length > 50)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            var foundLetter = db.SecretariatLetters.Where(t => t.IsConfirm == true && t.SiteSettingId == siteSettingId && t.Id == input.pKey && t.SecretariatLetterUsers.Any(tt => tt.UserId == userId)).FirstOrDefault();
            if (foundLetter == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return foundLetter;
        }

        public object GetList(SecretariatLetterUserMainGrid searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SecretariatLetterUsers
                .Where(t => t.SecretariatLetter.SiteSettingId == siteSettingId && t.SecretariatLetterId == searchInput.pKey);

            if (loginUserId.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.UserId == loginUserId || t.ByUserId == loginUserId).Where(t => t.SecretariatLetter.IsConfirm == true);

            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userFullname));
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.mobile) && searchInput.mobile.IsMobile())
                quiryResult = quiryResult.Where(t => t.User.Username == searchInput.mobile);

            int row = searchInput.skip;
            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    userFullname = t.User.Firstname + " " + t.User.Lastname,
                    type = t.Type,
                    createDate = t.CreateDate,
                    mobile = t.User.Mobile
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    t.userFullname,
                    type = t.type.GetEnumDisplayName(),
                    createDate = t.createDate.ToFaDate(),
                    t.mobile
                })
                .ToList()
            };
        }

        public void Update(long secretariatLetterId, string mobile, string fullname, int siteSettingId, long userId, SecretariatLetterUserType type)
        {
            if (secretariatLetterId > 0 && !string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(fullname) && siteSettingId > 0 && userId > 0)
            {
                var foundUserId = UserService.GetOrCreateByUsername(mobile, siteSettingId, fullname, userId);

                if (foundUserId.ToLongReturnZiro() > 0)
                {
                    var prevItems = db.SecretariatLetterUsers.Where(t => t.SecretariatLetterId == secretariatLetterId).ToList();
                    if (prevItems.Count > 0)
                    {
                        foreach (var item in prevItems)
                            db.Entry(item).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    db.Entry(new SecretariatLetterUser()
                    {
                        SecretariatLetterId = secretariatLetterId,
                        Type = type,
                        UserId = foundUserId.Value,
                        CreateDate = System.DateTime.Now
                    }).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
        }

        public ApiResult Delete(long? secretariatLetterId, long? id, int? siteSettingId, long? userId)
        {
            deleteValidation(secretariatLetterId, id, siteSettingId, userId);

            var foundItem = db.SecretariatLetterUsers
                .Where(t => t.SecretariatLetterId == secretariatLetterId && t.Id == id && t.SecretariatLetter.SiteSettingId == siteSettingId && t.ByUserId == userId && t.SecretariatLetter.IsConfirm == true).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            try
            {
                db.Entry(foundItem).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void deleteValidation(long? secretariatLetterId, long? id, int? siteSettingId, long? userId)
        {
            if (secretariatLetterId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (id.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
        }
    }
}
