using Microsoft.EntityFrameworkCore;
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
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class UserNotificationTrigerService : IUserNotificationTrigerService
    {
        readonly AccountDBContext db = null;
        readonly IUserNotificationTemplateService UserNotificationTemplateService = null;
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly IUserNotificationService UserNotificationService = null;
        public UserNotificationTrigerService(
            AccountDBContext db,
            IUserNotificationTemplateService UserNotificationTemplateService,
            IUserService UserService,
            IRoleService RoleService,
            IUserNotificationService UserNotificationService
            )
        {
            this.db = db;
            this.UserNotificationTemplateService = UserNotificationTemplateService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.UserNotificationService = UserNotificationService;
        }

        public ApiResult Create(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId)
        {
            CreateUpdateValidation(input, siteSettingId);

            db.Entry(new UserNotificationTriger()
            {
                RoleId = input.roleId,
                UserId = input.userId,
                UserNotificationType = input.type.Value,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public void CreateNotificationForUser(long? userId, UserNotificationType type, List<long> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink)
        {
            var foundTemplate = UserNotificationTemplateService.GetBy(type, siteSettingId);

            if (foundTemplate != null && !string.IsNullOrEmpty(foundTemplate.Subject) && !string.IsNullOrEmpty(foundTemplate.Description))
            {
                var foundTrigers = GetBy(type, siteSettingId);
                if (foundTrigers != null && foundTrigers.Count > 0)
                {
                    string userFullname = UserService.GetUserFullName(siteSettingId, userId);
                    string subject = replaceKeyword(foundTemplate.Subject, objectId, title, userFullname);
                    string description = replaceKeyword(foundTemplate.Description, objectId, title, userFullname);
                    if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(description))
                    {
                        List<int> roleIds = foundTrigers.Where(t => t.RoleId.ToIntReturnZiro() > 0).Select(t => t.RoleId.Value).ToList();
                        List<long> userIds = foundTrigers.Where(t => t.UserId.ToLongReturnZiro() > 0).Select(t => t.UserId.Value).ToList();
                        var roleUsers = RoleService.GetUsersBy(roleIds, siteSettingId, 1000);
                        roleUsers = roleUsers.Where(t => !userIds.Contains(t.userId)).ToList();
                        foreach (var fUserId in userIds)
                        {
                            UserNotificationService.Create(new UserNotification()
                            {
                                CreateDate = DateTime.Now,
                                Description = description.Replace("{{toUser}}", foundTrigers.Where(t => t.UserId == fUserId).Select(t => t.userFullname).FirstOrDefault()),
                                Subject = subject.Replace("{{toUser}}", foundTrigers.Where(t => t.UserId == fUserId).Select(t => t.userFullname).FirstOrDefault()),
                                FromUserId = userId,
                                ObjectId = objectId,
                                SiteSettingId = siteSettingId.Value,
                                Type = type,
                                UserId = fUserId,
                                TargetPageLink = openLink
                            }, siteSettingId);
                        }

                        foreach (var fUserId in roleUsers)
                        {
                            UserNotificationService.Create(new UserNotification()
                            {
                                CreateDate = DateTime.Now,
                                Description = description.Replace("{{toUser}}", fUserId.userFullname),
                                Subject = subject.Replace("{{toUser}}", fUserId.userFullname),
                                FromUserId = userId,
                                ObjectId = objectId,
                                SiteSettingId = siteSettingId.Value,
                                Type = type,
                                UserId = fUserId.userId,
                                TargetPageLink = openLink
                            }, siteSettingId);
                        }

                        UserNotificationService.SaveChange();
                    }
                }
            }
        }

        private List<UserNotificationTrigerURUVM> GetBy(UserNotificationType type, int? siteSettingId)
        {
            return db.UserNotificationTrigers
                .Where(t => t.UserNotificationType == type && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    t.RoleId,
                    t.UserId,
                    userFullname = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : ""
                })
                .ToList()
                .Select(t => new UserNotificationTrigerURUVM
                {
                    RoleId = t.RoleId,
                    UserId = t.UserId,
                    userFullname = t.userFullname
                })
                .ToList()
                ;
        }

        string replaceKeyword(string input, long? objectId, string title, string userFullname)
        {
            return (input + "").Replace("{{datetime}}", DateTime.Now.ToFaDate()).Replace("{{objectId}}", objectId + "").Replace("{{fromUser}}", userFullname).Replace("{{title}}", title);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {

            var foundItem = db.UserNotificationTrigers.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserNotificationTrigers
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.UserNotificationType,
                    roleId = t.RoleId,
                    userId = t.UserId,
                    userId_Title = t.UserId > 0 ? t.User.Username + "(" + t.User.Firstname + " " + t.User.Lastname + ")" : ""
                })
                .Take(1)
                .Select(t => new
                {
                    t.id,
                    type = (int)t.type,
                    roleId = t.roleId == null ? "" : t.roleId.ToString(),
                    userId = t.userId == null ? "" : t.userId.ToString(),
                    t.userId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserNotificationTrigerMainGridResultVM> GetList(UserNotificationTrigerMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserNotificationTrigerMainGrid();

            var qureResult = db.UserNotificationTrigers.Where(t => t.SiteSettingId == siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.UserNotificationType == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.roleName))
                qureResult = qureResult.Where(t => t.RoleId > 0 && t.Role.Title.Contains(searchInput.roleName));
            if (!string.IsNullOrEmpty(searchInput.userName))
                qureResult = qureResult.Where(t => t.UserId > 0 && (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userName));

            int row = searchInput.skip;

            return new GridResultVM<UserNotificationTrigerMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.UserNotificationType,
                    roleName = t.RoleId > 0 ? t.Role.Title : "",
                    userName = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : ""
                })
                .ToList()
                .Select(t => new UserNotificationTrigerMainGridResultVM()
                {
                    id = t.id,
                    row = ++row,
                    type = t.type.GetEnumDisplayName(),
                    roleName = t.roleName,
                    userName = t.userName
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId)
        {
            CreateUpdateValidation(input, siteSettingId);

            var foundItem = db.UserNotificationTrigers.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.RoleId = input.roleId;
            foundItem.UserId = input.userId;
            foundItem.UserNotificationType = input.type.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.roleId.ToIntReturnZiro() <= 0 && input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_User_Or_Role);
        }

        public UserNotificationType ConvertProposalFilledFormStatusToUserNotifiactionType(ProposalFilledFormStatus status)
        {
            switch (status)
            {
                case ProposalFilledFormStatus.New:
                    return UserNotificationType.ProposalFilledFormStatusChangedNew;
                case ProposalFilledFormStatus.W8ForConfirm:
                    return UserNotificationType.ProposalFilledFormStatusChangeW8ForConfirm;
                case ProposalFilledFormStatus.NeedSpecialist:
                    return UserNotificationType.ProposalFilledFormStatusChangeNeedSpecialist;
                case ProposalFilledFormStatus.Confirm:
                    return UserNotificationType.ProposalFilledFormStatusChangeConfirm;
                case ProposalFilledFormStatus.Issuing:
                    return UserNotificationType.ProposalFilledFormStatusChangeIssue;
                case ProposalFilledFormStatus.NotIssue:
                    return UserNotificationType.ProposalFilledFormStatusChangeNotIssue;
                default:
                    return UserNotificationType.ProposalFilledFormStatusChangedNew;
            }
        }
    }
}
