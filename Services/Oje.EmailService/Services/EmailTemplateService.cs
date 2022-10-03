using Microsoft.AspNetCore.Http;
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
    public class EmailTemplateService: IEmailTemplateService
    {
        readonly EmailServiceDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public EmailTemplateService
            (
                EmailServiceDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(EmailTemplateCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new EmailTemplate()
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

        private void createUpdateValidation(EmailTemplateCreateUpdateVM input, int? siteSettingId)
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
            if (db.EmailTemplates.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Type == input.type && t.ProposalFilledFormUserType == input.pffUserType))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.EmailTemplates
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.EmailTemplates.OrderByDescending(t => t.Id)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
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

        public GridResultVM<EmailTemplateMainGridResultVM> GetList(EmailTemplateMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new EmailTemplateMainGrid();

            var qureResult = db.EmailTemplates
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.subject))
                qureResult = qureResult.Where(t => t.Subject.Contains(searchInput.subject));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.pffUserType != null)
                qureResult = qureResult.Where(t => t.ProposalFilledFormUserType == searchInput.pffUserType);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<EmailTemplateMainGridResultVM>()
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
                    pffUserType = t.ProposalFilledFormUserType,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new EmailTemplateMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    subject = t.subject,
                    type = t.type.GetEnumDisplayName(),
                    pffUserType = t.pffUserType.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(EmailTemplateCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);
            var foundItem = db.EmailTemplates
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
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

        public List<EmailTemplate> GetBy(UserNotificationType type, int? siteSettingId)
        {
            return db.EmailTemplates.Where(t => t.Type == type && t.SiteSettingId == siteSettingId).ToList();
        }
    }
}
