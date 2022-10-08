using Microsoft.AspNetCore.Http;
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

namespace Oje.AccountService.Services
{
    public class UserNotificationTrigerService : IUserNotificationTrigerService
    {
        readonly AccountDBContext db = null;
        readonly IUserNotificationTemplateService UserNotificationTemplateService = null;
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly IUserNotificationService UserNotificationService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserNotificationTrigerService(
            AccountDBContext db,
            IUserNotificationTemplateService UserNotificationTemplateService,
            IUserService UserService,
            IRoleService RoleService,
            IUserNotificationService UserNotificationService,
            IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserNotificationTemplateService = UserNotificationTemplateService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.UserNotificationService = UserNotificationService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new UserNotificationTriger()
            {
                RoleId = input.roleId,
                UserId = input.userId,
                UserNotificationType = input.type.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public void CreateNotificationForUser(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink, object exteraParameter, bool isModal)
        {
            var foundTemplates = UserNotificationTemplateService.GetBy(type, siteSettingId);
            string userFullname = UserService.GetUserFullName(siteSettingId, userId);

            foreach (var foundTemplate in foundTemplates)
            {
                if (foundTemplate != null && !string.IsNullOrEmpty(foundTemplate.Subject) && !string.IsNullOrEmpty(foundTemplate.Description) && foundTemplate.ProposalFilledFormUserType == null)
                {
                    var foundTrigers = GetBy(type, siteSettingId);
                    if (foundTrigers != null && foundTrigers.Count > 0)
                    {
                        string subject = GlobalServices.replaceKeyword(foundTemplate.Subject, objectId, title, userFullname, exteraParameter);
                        string description = GlobalServices.replaceKeyword(foundTemplate.Description, objectId, title, userFullname, exteraParameter);
                        if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(description))
                        {
                            List<int> roleIds = foundTrigers.Where(t => t.RoleId.ToIntReturnZiro() > 0).Select(t => t.RoleId.Value).ToList();
                            List<long> userIds = foundTrigers.Where(t => t.UserId.ToLongReturnZiro() > 0).Select(t => t.UserId.Value).ToList();
                            userIds = userIds.GroupBy(t => t).Select(t => t.Key).ToList();
                            var roleUsers = RoleService.GetUsersBy(roleIds, siteSettingId, GlobalServices.MaxForNotify);
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
                                    TargetPageLink = openLink,
                                    IsModal = isModal
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
                                    TargetPageLink = openLink,
                                    IsModal = isModal
                                }, siteSettingId);
                            }
                        }
                    }
                }
                else if (foundTemplate != null && !string.IsNullOrEmpty(foundTemplate.Subject) && !string.IsNullOrEmpty(foundTemplate.Description) && foundTemplate.ProposalFilledFormUserType != null && exteraUserList != null)
                {
                    var foundTargetUsers = exteraUserList.Where(t => t.ProposalFilledFormUserType == foundTemplate.ProposalFilledFormUserType).ToList();
                    if (foundTargetUsers != null)
                    {
                        foreach (var foundTargetUser in foundTargetUsers)
                        {
                            string subject = GlobalServices.replaceKeyword(foundTemplate.Subject, objectId, title, userFullname, exteraParameter);
                            string description = GlobalServices.replaceKeyword(foundTemplate.Description, objectId, title, userFullname, exteraParameter);
                            UserNotificationService.Create(new UserNotification()
                            {
                                CreateDate = DateTime.Now,
                                Description = description.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                Subject = subject.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                FromUserId = userId,
                                ObjectId = objectId,
                                SiteSettingId = siteSettingId.Value,
                                Type = type,
                                UserId = foundTargetUser.userId,
                                TargetPageLink = openLink,
                                IsModal = isModal
                            }, siteSettingId);
                        }
                    }
                }
            }

            if (foundTemplates != null && foundTemplates.Count > 0)
                UserNotificationService.SaveChange();
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

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserNotificationTrigers
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
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
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.UserNotificationType,
                    roleId = t.RoleId,
                    userId = t.UserId,
                    userId_Title = t.UserId > 0 ? t.User.Username + "(" + t.User.Firstname + " " + t.User.Lastname + ")" : "",
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .Take(1)
                .Select(t => new
                {
                    t.id,
                    type = (int)t.type,
                    roleId = t.roleId == null ? "" : t.roleId.ToString(),
                    userId = t.userId == null ? "" : t.userId.ToString(),
                    t.userId_Title,
                    t.cSOWSiteSettingId,
                    t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserNotificationTrigerMainGridResultVM> GetList(UserNotificationTrigerMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserNotificationTrigerMainGrid();

            var qureResult = db.UserNotificationTrigers.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.UserNotificationType == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.roleName))
                qureResult = qureResult.Where(t => t.RoleId > 0 && t.Role.Title.Contains(searchInput.roleName));
            if (!string.IsNullOrEmpty(searchInput.userName))
                qureResult = qureResult.Where(t => t.UserId > 0 && (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userName));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

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
                    userName = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : "",
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new UserNotificationTrigerMainGridResultVM()
                {
                    id = t.id,
                    row = ++row,
                    type = t.type.GetEnumDisplayName(),
                    roleName = t.roleName,
                    userName = t.userName,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.UserNotificationTrigers
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.RoleId = input.roleId;
            foundItem.UserId = input.userId;
            foundItem.UserNotificationType = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.roleId.ToIntReturnZiro() <= 0 && input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_User_Or_Role);
            if (input.userId.ToLongReturnZiro() > 0 && !db.Users.Any(t => t.Id == input.userId && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }
    }
}
