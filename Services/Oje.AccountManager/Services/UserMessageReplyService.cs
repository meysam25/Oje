using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class UserMessageReplyService : IUserMessageReplyService
    {
        readonly AccountDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IUserService UserService = null;

        public UserMessageReplyService
            (
                AccountDBContext db,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService,
                IUserService UserService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
            this.UserService = UserService;
        }

        public void Create(UserMessageCreateVM input, long? loginUserId, int? siteSettingId, long? userMessageId)
        {
            createValidation(input, loginUserId, siteSettingId, userMessageId);

            db.Entry(new UserMessageReply()
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                FileUrl = input.mainFile != null && input.mainFile.Length > 0 ? UploadedFileService.UploadNewFile(FileType.UserMessageFile, input.mainFile, loginUserId, siteSettingId, userMessageId, ".jpg,.jpeg,.png,.pdf,.doc,.docx", true, null, null, input.userId) : null,
                Message = input.message,
                UserId = loginUserId.Value,
                UserMessageId = userMessageId.Value
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public object Create(UserMessageCreateVM input, int? siteSettingId, long? loginUserId)
        {
            input.userId = db.UserMessages.Where(t => t.Id == input.pKey).Select(t => t.FromUserId == loginUserId ? t.ToUserId : t.FromUserId).FirstOrDefault();
            Create(input, loginUserId, siteSettingId, input.pKey);

            UserNotifierService.Notify(loginUserId, UserNotificationType.AddNewMessage, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(input.userId.Value, ProposalFilledFormUserType.OwnerUser) }, input.pKey, "پاسخ جدید", siteSettingId, "/UserAccount/UserMessage/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetList(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new GlobalGridParentLong();

            var quiryResult = db.UserMessageReplies
                .Where(t => t.UserMessageId == searchInput.pKey && t.UserMessage.SiteSettingId == siteSettingId && (t.UserMessage.FromUserId == loginUserId || t.UserMessage.ToUserId == loginUserId));

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    fullname = t.User.Firstname + " " + t.User.Lastname,
                    createDate = t.CreateDate,
                    message = t.Message,
                    t.FileUrl
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    message = t.message,
                    t.fullname,
                    fileUrl = !string.IsNullOrEmpty(t.FileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.FileUrl : ""
                })
                .ToList()
            };
        }

        private void createValidation(UserMessageCreateVM input, long? loginUserId, int? siteSettingId, long? userMessageId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userMessageId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Message);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(".jpg,.jpeg,.png,.pdf,.doc,.docx"))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
        }
    }
}
