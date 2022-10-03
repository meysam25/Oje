using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;

namespace Oje.EmailService.Services
{
    public class EmailConfigService : IEmailConfigService
    {
        readonly EmailServiceDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public EmailConfigService(
                EmailServiceDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(EmailConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            if (input.isActive == true)
            {
                var allItems = db.EmailConfigs.Where(t => t.SiteSettingId == siteSettingId).ToList();
                foreach (var item in allItems)
                    item.IsActive = false;
            }

            db.Entry(new EmailConfig()
            {
                EnableSsl = input.enableSsl.ToBooleanReturnFalse(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Password = input.ePassword.Encrypt(),
                SiteSettingId = siteSettingId.Value,
                SmtpHost = input.smtpHost,
                SmtpPort = input.smtpPort.Value,
                Timeout = input.timeout.Value,
                Title = input.title,
                UseDefaultCredentials = input.useDefaultCredentials.ToBooleanReturnFalse(),
                Username = input.eUsername
            }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.EmailConfigs
                .Include(t => t.EmailSendingQueueErrors)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var errro in foundItem.EmailSendingQueueErrors)
                db.Entry(errro).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public EmailConfig GetActive(int? siteSettingId)
        {
            return db.EmailConfigs.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.EmailConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    eUsername = t.Username,
                    smtpPort = t.SmtpPort,
                    smtpHost = t.SmtpHost,
                    enableSsl = t.EnableSsl,
                    useDefaultCredentials = t.UseDefaultCredentials,
                    timeout = t.Timeout,
                    isActive = t.IsActive
                }).FirstOrDefault();
        }

        public GridResultVM<EmailConfigMainGridResultVM> GetList(EmailConfigMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new EmailConfigMainGrid();

            var qureResult = db.EmailConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.eUsername))
                qureResult = qureResult.Where(t => t.Username.Contains(searchInput.eUsername));
            if (searchInput.smtpPort.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.SmtpPort == searchInput.smtpPort);
            if (!string.IsNullOrEmpty(searchInput.smtpHost))
                qureResult = qureResult.Where(t => t.SmtpHost.Contains(searchInput.smtpHost));
            if (searchInput.enableSsl != null)
                qureResult = qureResult.Where(t => t.EnableSsl == searchInput.enableSsl);
            if (searchInput.timeout.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Timeout == searchInput.timeout);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<EmailConfigMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        title = t.Title,
                        eUsername = t.Username,
                        smtpPort = t.SmtpPort,
                        smtpHost = t.SmtpHost,
                        enableSsl = t.EnableSsl,
                        timeout = t.Timeout,
                        isActive = t.IsActive,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new EmailConfigMainGridResultVM
                    {
                        row = ++row,
                        enableSsl = t.enableSsl == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        eUsername = t.eUsername,
                        id = t.id,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        smtpHost = t.smtpHost,
                        smtpPort = t.smtpPort,
                        timeout = t.timeout,
                        title = t.title,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(EmailConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.EmailConfigs
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.EnableSsl = input.enableSsl.ToBooleanReturnFalse();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            if (!string.IsNullOrEmpty(input.ePassword))
                foundItem.Password = input.ePassword.Encrypt();
            foundItem.SmtpHost = input.smtpHost;
            foundItem.SmtpPort = input.smtpPort.Value;
            foundItem.Timeout = input.timeout.Value;
            foundItem.Title = input.title;
            foundItem.UseDefaultCredentials = input.useDefaultCredentials.ToBooleanReturnFalse();
            foundItem.Username = input.eUsername;

            if (foundItem.IsActive == true)
            {
                var allItems = db.EmailConfigs.Where(t => t.SiteSettingId == siteSettingId && t.Id != input.id).ToList();
                foreach (var item in allItems)
                    item.IsActive = false;
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(EmailConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.eUsername))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (input.id.ToIntReturnZiro() <= 0 && string.IsNullOrEmpty(input.ePassword))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (!string.IsNullOrEmpty(input.ePassword) && input.ePassword.Length > 30)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_30_Chars);
            if (input.smtpPort.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Prot);
            if (string.IsNullOrEmpty(input.smtpHost))
                throw BException.GenerateNewException(BMessages.Please_Enter_Host);
            if (input.timeout.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Timeout);

        }
    }
}
