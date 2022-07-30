using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
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
    public class UserMessageService : IUserMessageService
    {
        readonly AccountDBContext db = null;
        readonly IUserMessageReplyService UserMessageReplyService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IUserService UserService = null;

        public UserMessageService
            (
                AccountDBContext db,
                IUserMessageReplyService UserMessageReplyService,
                IUserNotifierService UserNotifierService,
                IUserService UserService
            )
        {
            this.db = db;
            this.UserMessageReplyService = UserMessageReplyService;
            this.UserNotifierService = UserNotifierService;
            this.UserService = UserService;
        }

        public ApiResult Create(UserMessageCreateVM input, long? loginUserId, int? siteSettingId)
        {
            createValidation(input, loginUserId, siteSettingId);

            var newItem = new UserMessage()
            {
                CreateDate = DateTime.Now,
                FromUserId = loginUserId.Value,
                ToUserId = input.userId.Value,
                SiteSettingId = siteSettingId.Value
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            UserMessageReplyService.Create(input, loginUserId, siteSettingId, newItem.Id);

            UserNotifierService.Notify(loginUserId, UserNotificationType.AddNewMessage, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(input.userId.Value, ProposalFilledFormUserType.OwnerUser) }, newItem.Id, "پیام جدید", siteSettingId, "/UserAccount/UserMessage/Index", new { message = input.message }, input.isModal.ToBooleanReturnFalse());

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(UserMessageCreateVM input, long? loginUserId, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (string.IsNullOrEmpty(input.message))
                throw BException.GenerateNewException(BMessages.Please_Enter_Message);
            if (input.message.Length > 4000)
                throw BException.GenerateNewException(BMessages.Message_Is_To_Long);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(".jpg,.jpeg,.png,.pdf,.doc,.docx"))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);

        }

        public GridResultVM<UserMessageMainGridResultVM> GetList(UserMessageMainGrid searchInput, long? loginUserId, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserMessageMainGrid();

            var quiryResult = db.UserMessages.Where(t => t.SiteSettingId == siteSettingId && (t.FromUserId == loginUserId || t.ToUserId == loginUserId));

            int row = searchInput.skip;

            return new GridResultVM<UserMessageMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    userFullname = loginUserId == t.FromUserId ? (t.ToUser.Firstname + " " + t.ToUser.Lastname) : (t.FromUser.Firstname + " " + t.FromUser.Lastname),
                    lastAnswerDate = t.UserMessageReplies.Any() ? t.UserMessageReplies.OrderByDescending(t => t.CreateDate).Select(tt => tt.CreateDate).FirstOrDefault() : t.CreateDate
                })
                .ToList()
                .Select(t => new UserMessageMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    userfullname = t.userFullname,
                    lastAnswerDate = t.lastAnswerDate.ToFaDate() + " " + t.lastAnswerDate.ToString("HH:mm")
                }).ToList()
            };
        }
    }
}
