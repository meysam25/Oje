using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;
using System.Net;
using System.Net.Mail;

namespace Oje.Security.Services
{
    public class DebugEmailService : IDebugEmailService
    {
        readonly SecurityDBContext db = null;
        public DebugEmailService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult CreateUpdate(DebugEmailCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.DebugEmails.FirstOrDefault();
            if (foundItem == null)
            {
                if (string.IsNullOrEmpty(input.password))
                    throw BException.GenerateNewException(BMessages.Please_Enter_Password);
                foundItem = new DebugEmail();
                db.Entry(foundItem).State = EntityState.Added;
            }

            foundItem.EnableSsl = input.isSSL.ToBooleanReturnFalse();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            if (!string.IsNullOrEmpty(input.password))
                foundItem.Password = input.password.Encrypt();
            foundItem.SmtpHost = input.smtpHost;
            foundItem.SmtpPort = input.smtpPort.ToIntReturnZiro();
            foundItem.Timeout = input.timeOut.ToIntReturnZiro();
            foundItem.UseDefaultCredentials = input.isDefaultCredentials.ToBooleanReturnFalse();
            foundItem.Username = input.username;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(DebugEmailCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);

            if (input.smtpPort.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(input.smtpHost))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public object Get()
        {
            return db.DebugEmails
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    isSSL = t.EnableSsl,
                    isActive = t.IsActive,
                    smtpHost = t.SmtpHost,
                    smtpPort = t.SmtpPort,
                    timeOut = t.Timeout,
                    isDefaultCredentials = t.UseDefaultCredentials,
                    username = t.Username
                })
                .FirstOrDefault();
        }

        public async Task<EmailResult> Send(string emailAddress, string subject, string message, DebugEmail foundConfig)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return new EmailResult() { isSuccess = false, message = "ایمیل نمی تواند خالی باشد" };
            if (!emailAddress.IsValidEmail())
                return new EmailResult() { isSuccess = false, message = "آدرس ایمیل صحیح نمی باشد" };
            if (string.IsNullOrEmpty(subject))
                return new EmailResult() { isSuccess = false, message = "ایمیل صحیح نمی باشد" };
            if (string.IsNullOrEmpty(message))
                return new EmailResult() { isSuccess = false, message = "پیغام نمی تواند خالی باشد" };

            var fromAddress = new MailAddress(foundConfig.Username, foundConfig.Username);
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

            return new EmailResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() };
        }

        public async Task SendEmail()
        {
            var foundConfig = db.DebugEmails.Where(t => t.IsActive == true).FirstOrDefault();
            if (foundConfig == null)
                return;
            List<string> ResiverEmailes = db.DebugEmailReceivers.Where(t => t.IsActive == true).Select(t => t.Email).ToList();
            if (ResiverEmailes == null || ResiverEmailes.Count == 0)
                return;
            var curDT = DateTime.Now.AddSeconds(-55);
            var allItems = db.Errors.Where(t => t.BMessageCode == null &&  (t.IsSuccessEmail == null || t.IsSuccessEmail == false) && t.LastTryDate == null && (t.CountTry == null || t.CountTry == 0)).ToList();
            if (allItems.Count == 0)
                allItems = db.Errors.Where(t => t.BMessageCode == null && (t.IsSuccessEmail == null || t.IsSuccessEmail == false) && t.LastTryDate != null &&  curDT > t.LastTryDate && (t.CountTry == null || t.CountTry <= 2)).ToList();
            foreach (var item in allItems)
            {
                item.LastTryDate = DateTime.Now;
                if (item.CountTry == null)
                    item.CountTry = 0;
                item.CountTry++;
            }

            if (allItems.Count == 0)
                return;

            db.SaveChanges();

            foreach (var item in allItems)
            {
                foreach (var email in ResiverEmailes)
                {
                    EmailResult resultSms = null;
                    try
                    {
                        resultSms = await Send(email, "خطای سیستمی", item.Message, foundConfig);
                    }
                    catch (Exception ex)
                    {
                        item.IsSuccessEmail = false;
                        item.LastEmailErrorMessage = ex.Message;
                        continue;
                    };
                    if (resultSms != null && resultSms.isSuccess == true)
                    {
                        item.IsSuccessEmail = true;
                    }
                    else if (resultSms != null)
                    {
                        item.IsSuccessEmail = false;
                        item.LastEmailErrorMessage = resultSms.message;
                    }
                    else
                    {
                        item.IsSuccessEmail = false;
                        item.LastEmailErrorMessage = "علت خطا مشخص نمی باشد";
                    }
                }
            }

            db.SaveChanges();
        }
    }
}
