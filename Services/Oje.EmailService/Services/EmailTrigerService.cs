using Microsoft.EntityFrameworkCore;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;

namespace Oje.EmailService.Services
{
    public class EmailTrigerService: IEmailTrigerService
    {
        readonly EmailServiceDBContext db = null;
        readonly IEmailTemplateService EmailTemplateService = null;
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly IEmailSendingQueueService EmailSendingQueueService = null;
        public EmailTrigerService(
                EmailServiceDBContext db,
                IEmailTemplateService EmailTemplateService,
                IUserService UserService,
                IRoleService RoleService,
                IEmailSendingQueueService EmailSendingQueueService
            )
        {
            this.db = db;
            this.EmailTemplateService = EmailTemplateService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.EmailSendingQueueService = EmailSendingQueueService;
        }

        public ApiResult Create(EmailTrigerCreateUpdateVM input, int? siteSettingId)
        {
            CreateUpdateValidation(input, siteSettingId);

            db.Entry(new EmailTriger()
            {
                RoleId = input.roleId,
                UserId = input.userId,
                Type = input.type.Value,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(EmailTrigerCreateUpdateVM input, int? siteSettingId)
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

        public ApiResult Delete(int? id, int? siteSettingID)
        {
            var foundItem = db.EmailTrigers.Where(t => t.SiteSettingId == siteSettingID && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingID)
        {
            return db.EmailTrigers
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingID && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
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
                    t.userId_Title,
                })
                .FirstOrDefault();
        }

        public GridResultVM<EmailTrigerMainGridResultVM> GetList(EmailTrigerMainGrid searchInput, int? siteSettingID)
        {
            if (searchInput == null)
                searchInput = new EmailTrigerMainGrid();

            var qureResult = db.EmailTrigers.Where(t => t.SiteSettingId == siteSettingID);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.roleName))
                qureResult = qureResult.Where(t => t.RoleId > 0 && t.Role.Title.Contains(searchInput.roleName));
            if (!string.IsNullOrEmpty(searchInput.userName))
                qureResult = qureResult.Where(t => t.UserId > 0 && (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userName));

            int row = searchInput.skip;

            return new GridResultVM<EmailTrigerMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    roleName = t.RoleId > 0 ? t.Role.Title : "",
                    userName = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : ""
                })
                .ToList()
                .Select(t => new EmailTrigerMainGridResultVM()
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

        public ApiResult Update(EmailTrigerCreateUpdateVM input, int? siteSettingID)
        {
            CreateUpdateValidation(input, siteSettingID);

            var foundItem = db.EmailTrigers.Where(t => t.SiteSettingId == siteSettingID && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.RoleId = input.roleId;
            foundItem.UserId = input.userId;
            foundItem.Type = input.type.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private List<UserEmailTrigerURUVM> GetBy(UserNotificationType type, int? siteSettingId)
        {
            return db.EmailTrigers
                .Where(t => t.Type == type && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    t.RoleId,
                    t.UserId
                })
                .ToList()
                .Select(t => new UserEmailTrigerURUVM
                {
                    RoleId = t.RoleId,
                    UserId = t.UserId
                })
                .ToList()
                ;
        }

        public void CreateEmailQue(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, object exteraParameter)
        {
            var foundTemplates = EmailTemplateService.GetBy(type, siteSettingId);
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
                            var allUserInfo = UserService.GetUserFullNameAndEmail(userIds, siteSettingId);
                            foreach (var user in allUserInfo)
                            {
                                if (!string.IsNullOrEmpty(user.userFullname) && !string.IsNullOrEmpty(user.email) && user.email.IsValidEmail())
                                {
                                    EmailSendingQueueService.Create(new EmailSendingQueue()
                                    {
                                        CreateDate = DateTime.Now,
                                        Email = user.email,
                                        Body = description.Replace("{{toUser}}", user.userFullname),
                                        Subject = subject.Replace("{{toUser}}", user.userFullname),
                                        FromUserId = userId,
                                        ObjectId = objectId,
                                        SiteSettingId = siteSettingId.Value,
                                        Type = type,
                                        ToUserId = user.userId,
                                    }, siteSettingId);
                                }
                            }

                            foreach (var user in roleUsers)
                            {
                                if (!string.IsNullOrEmpty(user.userFullname) && !string.IsNullOrEmpty(user.email) && user.email.IsValidEmail())
                                {
                                    EmailSendingQueueService.Create(new EmailSendingQueue()
                                    {
                                        CreateDate = DateTime.Now,
                                        Email = user.email,
                                        Body = description.Replace("{{toUser}}", user.userFullname),
                                        Subject = subject.Replace("{{toUser}}", user.userFullname),
                                        FromUserId = userId,
                                        ObjectId = objectId,
                                        SiteSettingId = siteSettingId.Value,
                                        Type = type,
                                        ToUserId = user.userId,
                                    }, siteSettingId);
                                }
                            }

                        }
                    }
                }
                else if (foundTemplate != null && !string.IsNullOrEmpty(foundTemplate.Subject) && !string.IsNullOrEmpty(foundTemplate.Description) && foundTemplate.ProposalFilledFormUserType != null && exteraUserList != null)
                {
                    var foundTargetUsers = exteraUserList.Where(t => t.ProposalFilledFormUserType == foundTemplate.ProposalFilledFormUserType && !string.IsNullOrEmpty(t.emaile) && t.emaile.IsValidEmail() == true).ToList();
                    if (foundTargetUsers != null)
                    {
                        foreach (var foundTargetUser in foundTargetUsers)
                        {
                            string subject = GlobalServices.replaceKeyword(foundTemplate.Subject, objectId, title, userFullname, exteraParameter);
                            string description = GlobalServices.replaceKeyword(foundTemplate.Description, objectId, title, userFullname, exteraParameter);
                            EmailSendingQueueService.Create(new EmailSendingQueue()
                            {
                                CreateDate = DateTime.Now,
                                Email = foundTargetUser.emaile,
                                Body = description.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                Subject = subject.Replace("{{toUser}}", foundTargetUser.fullUserName),
                                FromUserId = userId,
                                ObjectId = objectId,
                                SiteSettingId = siteSettingId.Value,
                                Type = type,
                                ToUserId = foundTargetUser.userId,
                            }, siteSettingId);
                        }
                    }
                }
            }

            if (foundTemplates != null && foundTemplates.Count > 0)
                EmailSendingQueueService.SaveChange();
        }
    }
}
