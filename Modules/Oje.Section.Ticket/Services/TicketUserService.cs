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
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Ticket.Services
{
    public class TicketUserService : ITicketUserService
    {
        readonly TicketDBContext db = null;
        readonly ITicketCategoryService TicketCategoryService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IUserNotifierService UserNotifierService = null;
        public TicketUserService
            (
                TicketDBContext db,
                ITicketCategoryService TicketCategoryService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService
            )
        {
            this.db = db;
            this.TicketCategoryService = TicketCategoryService;
            this.UploadedFileService = UploadedFileService;
            this.UserNotifierService = UserNotifierService;
        }

        public ApiResult Create(TicketUserCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            createValidation(input, siteSettingId, loginUserId);

            var newItem = new TicketUser()
            {
                CreateUserId = loginUserId.Value,
                CreateDate = DateTime.Now,
                Description = input.des,
                SiteSettingId = siteSettingId.Value,
                TicketCategoryId = input.subCId.ToIntReturnZiro(),
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainFile != null && input.mainFile.Length > 0)
            {
                newItem.FileUrl = UploadedFileService.UploadNewFile(FileType.TicketUser, input.mainFile, loginUserId, siteSettingId, newItem.Id, ".jpg,.jpeg,.png,.pdf,.doc,.docx,.pdf,.xlsx,.xls", true);
                db.SaveChanges();
            }

            UserNotifierService.Notify(loginUserId, UserNotificationType.NewTicket, null, newItem.Id, input.title, siteSettingId, "/Ticket/TicketUserAdmin/Index" );

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(TicketUserCreateUpdateVM input, int? siteSettingId, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 400)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_300_chars);
            if (input.subCId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Category);
            if (!TicketCategoryService.Exist(input.subCId, siteSettingId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(input.des))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.des.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);

        }

        public object GetList(TicketUserMainGrid searchInput, int? siteSettingId, long? loginUserId)
        {
            searchInput = searchInput ?? new TicketUserMainGrid();

            var quiryResult = db.TicketUsers.Where(t => t.SiteSettingId == siteSettingId && t.CreateUserId == loginUserId && t.IsDelete != true);

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
                    title = t.Title,
                    createDate = t.CreateDate,
                    updateDate = t.UpdateDate,
                    lastAnswerUserId = t.TicketUserAnswers.OrderByDescending(tt => tt.Id).Take(1).Select(tt => tt.CreateUserId).FirstOrDefault()
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    t.title,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    updateDate = t.updateDate != null ? t.updateDate.ToFaDate() + " " + t.updateDate.Value.ToString("HH:mm") : "",
                    isNotAnswer = t.lastAnswerUserId > 0 && t.lastAnswerUserId != loginUserId ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public bool Exist(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TicketUsers.Any(t => t.IsDelete != true && t.Id == id && t.SiteSettingId == siteSettingId && t.CreateUserId == loginUserId);
        }

        public string UpdateUserIdAndUdateDate(long? id, long? loginUserId)
        {
            var foundItem = db.TicketUsers.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem != null)
            {
                foundItem.UpdateDate = DateTime.Now;
                foundItem.UpdateUserId = loginUserId;
                db.SaveChanges();

                return foundItem.Title;
            }

            return null;
        }

        public List<TicketUserItemListVM> GetAsListBy(long? id, int? siteSettingId, long? loginUserId)
        {
            return db.TicketUsers.OrderByDescending(t => t.Id).Where(t => t.IsDelete != true && t.Id == id && t.SiteSettingId == siteSettingId && t.CreateUserId == loginUserId)
                .Select(t => new
                {
                    message = t.Description,
                    t.CreateDate,
                    createUsername = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    t.FileUrl
                })
                .ToList()
                .Select(t => new TicketUserItemListVM
                {
                    id = (long)-1,
                    message = t.message,
                    createDate = t.CreateDate.GetUserFrindlyDate(),
                    isMyMessage = true,
                    createUsername = t.createUsername,
                    fileUrl = (!string.IsNullOrEmpty(t.FileUrl) ? (GlobalConfig.FileAccessHandlerUrl + t.FileUrl) : "")
                })
                .ToList()
                ;
        }

        public GridResultVM<TicketUserAdminMainGridResultVM> GetListForAdmin(TicketUserAdminMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TicketUserAdminMainGrid();

            var quiryResult = db.TicketUsers.Where(t => t.SiteSettingId == siteSettingId && t.IsDelete != true);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isAnswer != null && searchInput.isAnswer == true)
                quiryResult = quiryResult.Where(t => t.TicketUserAnswers.Any() && t.TicketUserAnswers.OrderByDescending(tt => tt.Id).Take(1).Select(tt => tt.CreateUserId).FirstOrDefault() != t.CreateUserId);
            if (searchInput.isAnswer != null && searchInput.isAnswer == false)
                quiryResult = quiryResult.Where(t => !t.TicketUserAnswers.Any() || t.TicketUserAnswers.OrderByDescending(tt => tt.Id).Take(1).Select(tt => tt.CreateUserId).FirstOrDefault() == t.CreateUserId);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.updateDate) && searchInput.updateDate.ToEnDate() != null)
            {
                var targetDate = searchInput.updateDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.UpdateDate != null && t.UpdateDate.Value.Year == targetDate.Year && t.UpdateDate.Value.Month == targetDate.Month && t.UpdateDate.Value.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.categoryTitle))
                quiryResult = quiryResult.Where(t => t.TicketCategory.Title.Contains(searchInput.categoryTitle));
            if (!string.IsNullOrEmpty(searchInput.userfullname))
                quiryResult = quiryResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.userfullname));
            if (!string.IsNullOrEmpty(searchInput.updateUserFullname))
                quiryResult = quiryResult.Where(t => t.UpdateUserId > 0 && (t.UpdateUser.Firstname + " " + t.UpdateUser.Lastname).Contains(searchInput.updateUserFullname));


            int row = searchInput.skip;

            return new GridResultVM<TicketUserAdminMainGridResultVM>
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
                    createDate = t.CreateDate,
                    updateDate = t.UpdateDate,
                    t.CreateUserId,
                    lastAnswerUserId = t.TicketUserAnswers.OrderByDescending(tt => tt.Id).Take(1).Select(tt => tt.CreateUserId).FirstOrDefault(),
                    catTitle = t.TicketCategory.Title,
                    userfullname = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    updateUserFullname = t.UpdateUserId > 0 ? t.UpdateUser.Firstname + " " + t.UpdateUser.Lastname : ""
                })
                .ToList()
                .Select(t => new TicketUserAdminMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    updateDate = t.updateDate != null ? t.updateDate.ToFaDate() + " " + t.updateDate.Value.ToString("HH:mm") : "",
                    isAnswer = t.lastAnswerUserId > 0 && t.lastAnswerUserId == t.CreateUserId ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    categoryTitle = t.catTitle,
                    userfullname = t.userfullname,
                    updateUserFullname = t.updateUserFullname
                })
                .ToList()
            };
        }

        public bool Exist(long? id, int? siteSettingId)
        {
            return db.TicketUsers.Any(t => t.IsDelete != true && t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public List<TicketUserItemListVM> GetAsListBy(long? id, int? siteSettingId)
        {
            return db.TicketUsers.OrderByDescending(t => t.Id).Where(t => t.IsDelete != true && t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    message = t.Description,
                    t.CreateDate,
                    createUsername = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    t.FileUrl
                })
                .ToList()
                .Select(t => new TicketUserItemListVM
                {
                    id = -1,
                    message = t.message,
                    createDate = t.CreateDate.GetUserFrindlyDate(),
                    isMyMessage = true,
                    createUsername = t.createUsername,
                    fileUrl = (!string.IsNullOrEmpty(t.FileUrl) ? (GlobalConfig.FileAccessHandlerUrl + t.FileUrl) : "")
                })
                .ToList()
                ;
        }
    }
}
