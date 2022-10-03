using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;

namespace Oje.EmailService.Services
{
    public class EmailSendingQueueService: IEmailSendingQueueService
    {
        readonly EmailServiceDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly IEmailSenderService EmailSenderService = null;
        readonly IEmailSendingQueueErrorService EmailSendingQueueErrorService = null;

        public EmailSendingQueueService(
                EmailServiceDBContext db,
                IHttpContextAccessor HttpContextAccessor,
                IEmailSenderService EmailSenderService,
                IEmailSendingQueueErrorService EmailSendingQueueErrorService
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
            this.EmailSenderService = EmailSenderService;
            this.EmailSendingQueueErrorService = EmailSendingQueueErrorService;
        }

        public void Create(EmailSendingQueue smsSendingQueue, int? siteSettingId)
        {
            var foundIp = HttpContextAccessor.GetIpAddress();
            if (foundIp != null)
            {
                smsSendingQueue.Ip1 = foundIp.Ip1;
                smsSendingQueue.Ip2 = foundIp.Ip2;
                smsSendingQueue.Ip3 = foundIp.Ip3;
                smsSendingQueue.Ip4 = foundIp.Ip4;
            }

            db.Entry(smsSendingQueue).State = EntityState.Added;
        }

        public object GetList(EmailSendingQueueMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.EmailSendingQueues
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.email))
                qureResult = qureResult.Where(t => t.Email == searchInput.email);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.lTryDate) && searchInput.lTryDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.lTryDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.LastTryDate != null && t.LastTryDate.Value.Year == targetDate.Year && t.LastTryDate.Value.Month == targetDate.Month && t.LastTryDate.Value.Day == targetDate.Day);
            }
            if (searchInput.countTry != null)
                qureResult = qureResult.Where(t => t.CountTry == searchInput.countTry);
            if (searchInput.isSuccess != null)
                qureResult = qureResult.Where(t => t.IsSuccess == searchInput.isSuccess);
            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.GetIpSections() != null)
            {
                var ipSections = searchInput.ip.GetIpSections();
                qureResult = qureResult.Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            var row = searchInput.skip;

            return new
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        type = t.Type,
                        email = t.Email,
                        createDate = t.CreateDate,
                        lTryDate = t.LastTryDate,
                        countTry = t.CountTry,
                        isSuccess = t.IsSuccess,
                        ip1 = t.Ip1,
                        ip2 = t.Ip2,
                        ip3 = t.Ip3,
                        ip4 = t.Ip4,
                        lastError = t.EmailSendingQueueErrors.OrderByDescending(tt => tt.CreateDate).Select(tt => tt.Description).FirstOrDefault(),
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new
                    {
                        t.id,
                        row = ++row,
                        type = t.type.GetEnumDisplayName(),
                        t.email,
                        createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                        lTryDate = t.lTryDate != null ? (t.lTryDate.ToFaDate() + " " + t.lTryDate.Value.ToString("hh:mm")) : "",
                        t.countTry,
                        isSuccess = t.isSuccess == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                        ip = t.ip1 + "." + t.ip2 + "." + t.ip3 + "." + t.ip4,
                        t.lastError,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public void SaveChange()
        {
            db.SaveChanges();
        }

        public async Task SendEmail()
        {
            var curDT = DateTime.Now.AddSeconds(-55);
            var allItems = db.EmailSendingQueues.Where(t => t.LastTryDate == null && t.IsSuccess == false && t.CountTry == 0).ToList();
            if (allItems.Count == 0)
                allItems = db.EmailSendingQueues.Where(t => t.LastTryDate != null && t.IsSuccess == false && curDT > t.LastTryDate && t.CountTry <= 2).ToList();
            foreach (var item in allItems)
                item.LastTryDate = DateTime.Now;

            db.SaveChanges();

            foreach (var item in allItems)
            {
                EmailResult resultSms = null;
                try
                {
                    resultSms = await EmailSenderService.Send(item.Email, item.Subject, item.Body, item.SiteSettingId);
                }
                catch (Exception ex)
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    EmailSendingQueueErrorService.Create(item.Id, DateTime.Now, ex.Message, null);
                    continue;
                };
                if (resultSms != null && resultSms.isSuccess == true)
                {
                    item.IsSuccess = true;
                }
                else if (resultSms != null)
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    EmailSendingQueueErrorService.Create(item.Id, DateTime.Now, resultSms.message, resultSms.cId);
                }
                else
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    EmailSendingQueueErrorService.Create(item.Id, DateTime.Now, "علت خطا مشخص نمی باشد", null);
                }
            }

            db.SaveChanges();
        }
    }
}
