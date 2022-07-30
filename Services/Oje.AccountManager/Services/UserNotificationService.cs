using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Hubs;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        readonly AccountDBContext db = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserService UserService = null;
        readonly IHubContext<NotificationHub> NotificationHubContext = null;

        public UserNotificationService
            (
                AccountDBContext db,
                ISiteSettingService SiteSettingService,
                IUserService UserService,
                IHubContext<NotificationHub> NotificationHubContext
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.NotificationHubContext = NotificationHubContext;
        }

        public void Create(UserNotification userNotification, int? siteSettingId)
        {
            if (userNotification != null && siteSettingId.ToIntReturnZiro() > 0 && userNotification.UserId > 0 && !string.IsNullOrEmpty(userNotification.Subject) && !string.IsNullOrEmpty(userNotification.Description))
            {
                db.Entry(userNotification).State = EntityState.Added;
                NotificationHub.SendNotification(NotificationHubContext, userNotification.Subject, userNotification.Description, new List<long>() { userNotification.UserId }, siteSettingId.ToIntReturnZiro(), userNotification.IsModal.ToBooleanReturnFalse(), userNotification.Type);
            }
        }

        public object GetBy(string id, long? userId, int? siteSettingId)
        {
            if (string.IsNullOrEmpty(id))
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (id.IndexOf("_") == -1)
                throw BException.GenerateNewException(BMessages.Not_Found);
            long idUserId = id.Split('_')[0].ToLongReturnZiro();
            long cdTick = id.Split('_')[1].ToLongReturnZiro();
            var dt = new DateTime(cdTick);
            UserNotificationType type = (UserNotificationType)id.Split('_')[2].ToIntReturnZiro();
            if (idUserId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (cdTick <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var canSeeAllItems = UserService.CanSeeAllItems(userId.ToLongReturnZiro());
            if (canSeeAllItems != true)
            {
                var foundNotification = db.UserNotifications
                    .Where(t => t.SiteSettingId == siteSettingId && t.UserId == idUserId && t.Type == type &&
                                t.CreateDate == dt)
                    .getWhereUserIdMultiLevelForUserOwnerShip<UserNotification, User>(userId.ToLongReturnZiro(), canSeeAllItems)
                    .FirstOrDefault();

                if (foundNotification == null)
                    throw BException.GenerateNewException(BMessages.Not_Found);

                if (idUserId == userId)
                {
                    foundNotification.ViewDate = DateTime.Now;
                    db.SaveChanges();
                }

                return new { description = foundNotification.Description };
            }
            else
            {
                var foundNotification = db.UserNotifications
                    .Where(t => t.SiteSettingId == siteSettingId && t.UserId == idUserId && t.Type == type &&
                                t.CreateDate == dt)
                        .FirstOrDefault();
                if (foundNotification == null)
                    throw BException.GenerateNewException(BMessages.Not_Found);
                if (idUserId == userId)
                {
                    foundNotification.ViewDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new { description = foundNotification.Description };
            }


            throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public GridResultVM<UserNotificationMainGridResultVM> GetList(UserNotificationMainGrid searchInput, long? userId, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserNotificationMainGrid();
            var canSeeAllItems = UserService.CanSeeAllItems(userId.ToLongReturnZiro());
            var qureResult = db.UserNotifications.Where(t => t.SiteSettingId == siteSettingId);
            if (searchInput.justMyNotification == true)
                qureResult = qureResult.Where(t => t.UserId == userId);
            else
                qureResult = qureResult.getWhereUserIdMultiLevelForUserOwnerShip<UserNotification, User>(userId.ToLongReturnZiro(), canSeeAllItems);

            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.fromUser))
                qureResult = qureResult.Where(t => t.FromUserId > 0 && (t.FromUser.Firstname + " " + t.FromUser.Lastname).Contains(searchInput.fromUser));
            if (!string.IsNullOrEmpty(searchInput.toUser))
                qureResult = qureResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.toUser));
            if (!string.IsNullOrEmpty(searchInput.subject))
                qureResult = qureResult.Where(t => t.Subject.Contains(searchInput.subject));
            if (!string.IsNullOrEmpty(searchInput.description))
                qureResult = qureResult.Where(t => t.Description.Contains(searchInput.description));
            if (!string.IsNullOrEmpty(searchInput.viewDate))
            {
                var targetDate = searchInput.viewDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.ViewDate != null && t.ViewDate.Value.Year == targetDate.Year && t.ViewDate.Value.Month == targetDate.Month && t.ViewDate.Value.Day == targetDate.Day);
            }
            if (searchInput.notSeen == true)
                qureResult = qureResult.Where(t => t.ViewDate == null);
            else if (searchInput.notSeen == false)
                qureResult = qureResult.Where(t => t.ViewDate != null);

            int row = searchInput.skip;

            return new GridResultVM<UserNotificationMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.CreateDate).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    createDate = t.CreateDate,
                    type = t.Type,
                    fromUser = t.FromUserId > 0 ? t.FromUser.Firstname + " " + t.FromUser.Lastname : "",
                    toUser = t.User.Firstname + " " + t.User.Lastname,
                    subject = t.Subject,
                    viewDate = t.ViewDate,
                    t.UserId,
                    link = t.TargetPageLink,
                    notSeen = t.ViewDate == null
                })
                .ToList()
                .Select(t => new UserNotificationMainGridResultVM
                {
                    row = ++row,
                    id = t.UserId + "_" + t.createDate.Ticks + "_" + ((int)t.type),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    type = t.type.GetEnumDisplayName(),
                    fromUser = t.fromUser,
                    subject = t.subject,
                    toUser = t.toUser,
                    viewDate = t.viewDate != null ? t.viewDate.ToFaDate() : "",
                    justMyNotification = t.UserId == userId ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    link = !string.IsNullOrEmpty(t.link) ? t.link : "",
                    notSeen = t.notSeen == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                })
                .ToList()
            };
        }

        public int? GetUserUnreadNotificationCount(long? userId)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            int result = db.UserNotifications.Where(t => t.UserId == userId && t.SiteSettingId == siteSettingId && t.ViewDate == null).Count();
            return result == 0 ? null : result;
        }

        public void SaveChange()
        {
            try { db.SaveChanges(); } catch { }
        }
    }
}
