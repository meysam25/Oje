using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.View;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        readonly IEmailConfigService EmailConfigService = null;
        public EmailSenderService(IEmailConfigService EmailConfigService)
        {
            this.EmailConfigService = EmailConfigService;
        }

        public async Task<EmailResult> Send(string emailAddress, string subject, string message, int? siteSettingId)
        {
            var foundConfig = EmailConfigService.GetActive(siteSettingId);
            if (foundConfig == null)
                return new EmailResult() { isSuccess = false, message = "تنظیمات یافت نشد" };
            if (string.IsNullOrEmpty(emailAddress))
                return new EmailResult() { isSuccess = false, message = "ایمیل نمی تواند خالی باشد" };
            if (!emailAddress.IsValidEmail())
                return new EmailResult() { isSuccess = false, message = "آدرس ایمیل صحیح نمی باشد" };
            if (string.IsNullOrEmpty(subject))
                return new EmailResult() { isSuccess = false, message = "ایمیل صحیح نمی باشد" };
            if (string.IsNullOrEmpty(message))
                return new EmailResult() { isSuccess = false, message = "پیغام نمی تواند خالی باشد" };

            var fromAddress = new MailAddress(foundConfig.Username, !string.IsNullOrEmpty(foundConfig.Title) ? foundConfig.Title : foundConfig.Username);
            var toAddress = new MailAddress(emailAddress, emailAddress);

            using (var smtp = new SmtpClient
            {
                Host = foundConfig.SmtpHost,
                Port = foundConfig.SmtpPort,
                EnableSsl = foundConfig.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = foundConfig.UseDefaultCredentials,
                Credentials = new NetworkCredential(fromAddress.Address, foundConfig.Password.Decrypt()),
                Timeout = foundConfig.Timeout
            })
            {
                using (var mailMessage = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = message, IsBodyHtml = true, })
                {
                    await smtp.SendMailAsync(mailMessage);
                }
            }

            return new EmailResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), cId = foundConfig?.Id };
        }
    }
}
