using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class SmsTrigerService : ISmsTrigerService
    {
        readonly SmsDBContext db = null;
        readonly ISmsTemplateService SmsTemplateService = null;
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly ISmsSendingQueueService SmsSendingQueueService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public SmsTrigerService(
                SmsDBContext db,
                ISmsTemplateService SmsTemplateService,
                IUserService UserService,
                IRoleService RoleService,
                ISmsSendingQueueService SmsSendingQueueService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SmsTemplateService = SmsTemplateService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateSmsTrigerVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new SmsTriger()
            {
                RoleId = input.roleId,
                UserId = input.userId,
                UserNotificationType = input.type.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(CreateUpdateSmsTrigerVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.roleId.ToIntReturnZiro() <= 0 && input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_User_Or_Role);
            if (input.userId.ToLongReturnZiro() > 0 && !db.Users.Any(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId) && t.Id == input.userId))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.SmsTrigers
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SmsTrigers
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

        public GridResultVM<SmsTrigerMainGridResultVM> GetList(SmsTrigerMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new SmsTrigerMainGrid();

            var qureResult = db.SmsTrigers.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.UserNotificationType == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.roleName))
                qureResult = qureResult.Where(t => t.RoleId > 0 && t.Role.Title.Contains(searchInput.roleName));
            if (!string.IsNullOrEmpty(searchInput.userName))
                qureResult = qureResult.Where(t => t.UserId > 0 && (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userName));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<SmsTrigerMainGridResultVM>()
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
                .Select(t => new SmsTrigerMainGridResultVM()
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

        public ApiResult Update(CreateUpdateSmsTrigerVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.SmsTrigers
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

        private List<UserSmsTrigerURUVM> GetBy(UserNotificationType type, int? siteSettingId)
        {
            return db.SmsTrigers
                .Where(t => t.UserNotificationType == type && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    t.RoleId,
                    t.UserId
                })
                .ToList()
                .Select(t => new UserSmsTrigerURUVM
                {
                    RoleId = t.RoleId,
                    UserId = t.UserId
                })
                .ToList()
                ;
        }

        public void CreateSmsQue(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, object exteraParameter)
        {
            var foundTemplates = SmsTemplateService.GetBy(type, siteSettingId);
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
                            var userIds = foundTrigers.Where(t => t.UserId.ToLongReturnZiro() > 0).Select(t => t.UserId.ToLongReturnZiro()).ToList();
                            userIds = userIds.GroupBy(t => t).Select(t => t.Key).ToList();
                            var roleUsers = RoleService.GetUsersBy(roleIds, siteSettingId, GlobalServices.MaxForNotify);
                            roleUsers = roleUsers.Where(t => !userIds.Contains(t.userId)).ToList();
                            var allUserInfo = UserService.GetUserFullNameAndMobile(userIds, siteSettingId);
                            foreach (var user in allUserInfo)
                            {
                                if (!string.IsNullOrEmpty(user.userFullname) && !string.IsNullOrEmpty(user.mobile) && user.mobile.IsMobile())
                                {
                                    SmsSendingQueueService.Create(new SmsSendingQueue()
                                    {
                                        CreateDate = DateTime.Now,
                                        MobileNumber = user.mobile,
                                        Body = description.Replace("{{toUser}}", user.userFullname),
                                        Subject = subject.Replace("{{toUser}}", user.userFullname),
                                        FromUserId = userId,
                                        ObjectId = objectId,
                                        SiteSettingId = siteSettingId.Value,
                                        Type = type,
                                        ToUserId = user.userId,
                                    }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(), HttpContextAccessor.HttpContext?.GetLoginUser()?.roles?.Any(t => t == "user"));
                                }
                            }

                            foreach (var user in roleUsers)
                            {
                                if (!string.IsNullOrEmpty(user.userFullname) && !string.IsNullOrEmpty(user.mobile) && user.mobile.IsMobile())
                                {
                                    SmsSendingQueueService.Create(new SmsSendingQueue()
                                    {
                                        CreateDate = DateTime.Now,
                                        MobileNumber = user.mobile,
                                        Body = description.Replace("{{toUser}}", user.userFullname),
                                        Subject = subject.Replace("{{toUser}}", user.userFullname),
                                        FromUserId = userId,
                                        ObjectId = objectId,
                                        SiteSettingId = siteSettingId.Value,
                                        Type = type,
                                        ToUserId = user.userId,
                                    }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(), HttpContextAccessor.HttpContext?.GetLoginUser()?.roles?.Any(t => t == "user"));
                                }
                            }
                        }
                    }
                }
                else if (foundTemplate != null && !string.IsNullOrEmpty(foundTemplate.Subject) && !string.IsNullOrEmpty(foundTemplate.Description) && foundTemplate.ProposalFilledFormUserType != null && exteraUserList != null)
                {
                    var foundTargetUsers = exteraUserList.Where(t => t.ProposalFilledFormUserType == foundTemplate.ProposalFilledFormUserType && !string.IsNullOrEmpty(t.mobile) && t.mobile.IsMobile() == true).ToList();
                    if(foundTargetUsers != null)
                    {
                        foreach(var foundTargetUser in foundTargetUsers)
                        {
                            string subject = GlobalServices.replaceKeyword(foundTemplate.Subject, objectId, title, userFullname, exteraParameter);
                            string description = GlobalServices.replaceKeyword(foundTemplate.Description, objectId, title, userFullname, exteraParameter);
                            SmsSendingQueueService.Create(new SmsSendingQueue()
                            {
                                CreateDate = DateTime.Now,
                                MobileNumber = foundTargetUser.mobile,
                                Body = description.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                Subject = subject.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                FromUserId = userId,
                                ObjectId = objectId,
                                SiteSettingId = siteSettingId.Value,
                                Type = type,
                                ToUserId = foundTargetUser.userId,
                            }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(),  HttpContextAccessor.HttpContext?.GetLoginUser()?.roles?.Any(t => t == "user"));
                        }
                    }
                }
            }

            if(foundTemplates != null && foundTemplates.Count > 0)
                SmsSendingQueueService.SaveChange();
        }
    }
}
