using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Models.DB;
using Oje.Section.Ticket.Models.View;
using Oje.Section.Ticket.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.Ticket.Services
{
    public class TicketUserAnswerService : ITicketUserAnswerService
    {
        readonly string validFileExtension = ".jpg,.jpeg,.png,.pdf,.doc,.docx,.pdf,.xlsx,.xls";

        readonly TicketDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly ITicketUserService TicketUserService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TicketUserAnswerService
            (
                TicketDBContext db,
                IUploadedFileService UploadedFileService,
                ITicketUserService TicketUserService,
                IUserNotifierService UserNotifierService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.TicketUserService = TicketUserService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(TicketUserAnswerCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createValidation(input, siteSettingId, loginUserId);
            if (!TicketUserService.Exist(input.pKey, siteSettingId, loginUserId))
                throw BException.GenerateNewException(BMessages.Not_Found);

            var newItem = new TicketUserAnswer()
            {
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.answer,
                SiteSettingId = siteSettingId.Value,
                TicketUserId = input.pKey.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
            {
                newItem.FileUrl = UploadedFileService.UploadNewFile(FileType.TicketUser, input.mainFile, loginUserId, siteSettingId, newItem.Id, ".jpg,.jpeg,.png,.pdf,.doc,.docx,.pdf,.xlsx,.xls", true);
                db.SaveChanges();
            }

            string ticketTitle =  TicketUserService.UpdateUserIdAndUdateDate(input.pKey, loginUserId);

            UserNotifierService.Notify(loginUserId, UserNotificationType.NewTicket, null, newItem.Id, ticketTitle, siteSettingId, "/Ticket/TicketUserAdmin/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(TicketUserAnswerCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.answer))
                throw BException.GenerateNewException(BMessages.Please_Enter_Answer);
            if (input.answer.Length > 4000)
                throw BException.GenerateNewException(BMessages.Answer_Length_Can_Not_Be_More_Then_4000);
            if (input.mainFile != null && input.mainFile.Length > 0 && !input.mainFile.IsValidExtension(validFileExtension))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

        }

        public object GetList(TicketUserAnswerMainGrid searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new TicketUserAnswerMainGrid();

            var quiryResult = db.TicketUserAnswers.Where(t => t.SiteSettingId == siteSettingId && t.TicketUser.SiteSettingId == siteSettingId && t.TicketUserId == searchInput.pKey && t.TicketUser.CreateUserId == loginUserId);

            var firstItem = TicketUserService.GetAsListBy(searchInput.pKey, siteSettingId, loginUserId);



            return new
            {
                total = quiryResult.Count() + 1,
                data =
                firstItem
                .Select(t => new
                {
                    t.id,
                    t.message,
                    t.createDate,
                    t.isMyMessage,
                    t.createUsername,
                    t.fileUrl
                })
                .ToList()
                .Union(
                quiryResult
                .OrderBy(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    message = t.Description,
                    t.CreateDate,
                    tCreateUserId = t.TicketUser.CreateUserId,
                    t.CreateUserId,
                    createUsername = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    t.FileUrl
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.message,
                    createDate = t.CreateDate.GetUserFrindlyDate(),
                    isMyMessage = t.tCreateUserId == t.CreateUserId,
                    t.createUsername,
                    fileUrl = (!string.IsNullOrEmpty(t.FileUrl) ? (GlobalConfig.FileAccessHandlerUrl + t.FileUrl) : "")
                })
                .ToList()
                ).ToList()
            };
        }

        public ApiResult CreateForAdmin(TicketUserAnswerCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createValidation(input, siteSettingId, loginUserId);
            if (!TicketUserService.Exist(input.pKey, siteSettingId))
                throw BException.GenerateNewException(BMessages.Not_Found);

            var newItem = new TicketUserAnswer()
            {
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.answer,
                SiteSettingId = siteSettingId.Value,
                TicketUserId = input.pKey.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
            {
                newItem.FileUrl = UploadedFileService.UploadNewFile(FileType.TicketUser, input.mainFile, loginUserId, siteSettingId, newItem.Id, ".jpg,.jpeg,.png,.pdf,.doc,.docx,.pdf,.xlsx,.xls", true);
                db.SaveChanges();
            }

            TicketUserService.UpdateUserIdAndUdateDate(input.pKey, loginUserId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListForAdmin(TicketUserAnswerMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TicketUserAnswerMainGrid();

            var quiryResult = db.TicketUserAnswers
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.TicketUserId == searchInput.pKey);

            var firstItem = TicketUserService.GetAsListBy(searchInput.pKey, siteSettingId);

            return new
            {
                total = quiryResult.Count() + 1,
                data =
                firstItem
                .Select(t => new
                {
                    t.id,
                    t.message,
                    t.createDate,
                    isMyMessage = false,
                    t.createUsername,
                    t.fileUrl
                })
                .ToList()
                .Union(
                quiryResult
                .OrderBy(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    message = t.Description,
                    t.CreateDate,
                    tCreateUserId = t.TicketUser.CreateUserId,
                    t.CreateUserId,
                    createUsername = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    t.FileUrl
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.message,
                    createDate = t.CreateDate.GetUserFrindlyDate(),
                    isMyMessage = t.tCreateUserId != t.CreateUserId,
                    t.createUsername,
                    fileUrl = (!string.IsNullOrEmpty(t.FileUrl) ? (GlobalConfig.FileAccessHandlerUrl + t.FileUrl) : "")
                })
                .ToList()
                ).ToList()
            };
        }
    }
}
