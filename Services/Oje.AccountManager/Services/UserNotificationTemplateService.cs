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
    public class UserNotificationTemplateService : IUserNotificationTemplateService
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public UserNotificationTemplateService
            (
                AccountDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateUserNotificationTemplateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new UserNotificationTemplate()
            {
                Type = input.type.Value,
                Description = input.description,
                Subject = input.subject,
                SiteSettingId = siteSettingId.Value,
                ProposalFilledFormUserType = input.pffUserType
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserNotificationTemplates
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<UserNotificationTemplate> GetBy(UserNotificationType type, int? siteSettingId)
        {
            return db.UserNotificationTemplates.Where(t => t.SiteSettingId == siteSettingId && t.Type == type).ToList();
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserNotificationTemplates.OrderByDescending(t => t.Id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    subject = t.Subject,
                    description = t.Description,
                    pffUserType = t.ProposalFilledFormUserType
                })
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    t.id,
                    type = (int)t.type,
                    t.subject,
                    t.description,
                    pffUserType = t.pffUserType != null ? ((int)t.pffUserType).ToString() : ""
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserNotificationTemplateMainGridResultVM> GetList(UserNotificationTemplateMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserNotificationTemplateMainGrid();

            var qureResult = db.UserNotificationTemplates.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.subject))
                qureResult = qureResult.Where(t => t.Subject.Contains(searchInput.subject));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.pffUserType != null)
                qureResult = qureResult.Where(t => t.ProposalFilledFormUserType == searchInput.pffUserType);

            int row = searchInput.skip;

            return new GridResultVM<UserNotificationTemplateMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    subject = t.Subject,
                    type = t.Type,
                    pffUserType = t.ProposalFilledFormUserType
                })
                .ToList()
                .Select(t => new UserNotificationTemplateMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    subject = t.subject,
                    type = t.type.GetEnumDisplayName(),
                    pffUserType = t.pffUserType != null ? t.pffUserType.GetEnumDisplayName() : ""
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateUserNotificationTemplateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);
            var foundItem = db.UserNotificationTemplates
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Type = input.type.Value;
            foundItem.Description = input.description;
            foundItem.Subject = input.subject;
            foundItem.ProposalFilledFormUserType = input.pffUserType;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdateUserNotificationTemplateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (string.IsNullOrEmpty(input.subject))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.subject.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (db.UserNotificationTemplates.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Type == input.type && t.ProposalFilledFormUserType == input.pffUserType))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }
    }
}
