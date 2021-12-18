using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class SmsSendingQueueService : ISmsSendingQueueService
    {
        readonly SmsDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public SmsSendingQueueService(
                SmsDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public void Create(SmsSendingQueue smsSendingQueue, int? siteSettingId)
        {
            var foundIp = HttpContextAccessor.GetIpAddress();
            if(foundIp != null)
            {
                smsSendingQueue.Ip1 = foundIp.Ip1;
                smsSendingQueue.Ip2 = foundIp.Ip2;
                smsSendingQueue.Ip3 = foundIp.Ip3;
                smsSendingQueue.Ip4 = foundIp.Ip4;
            }

            db.Entry(smsSendingQueue).State = EntityState.Added;
        }

        public object GetList(SmsSendingQueueMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.SmsSendingQueues.Where(t => t.SiteSettingId == siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.mobile))
                qureResult = qureResult.Where(t => t.MobileNumber == searchInput.mobile);
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
                        mobile = t.MobileNumber,
                        createDate = t.CreateDate,
                        lTryDate = t.LastTryDate,
                        countTry = t.CountTry,
                        isSuccess = t.IsSuccess,
                        ip1 = t.Ip1,
                        ip2 = t.Ip2,
                        ip3 = t.Ip3,
                        ip4 = t.Ip4,
                        lastError = t.SmsSendingQueueErrors.OrderByDescending(tt => tt.CreateDate).Select(tt => tt.Description).FirstOrDefault()
                    })
                    .ToList()
                    .Select(t => new
                    {
                        t.id,
                        row = ++row,
                        type = t.type.GetEnumDisplayName(),
                        t.mobile,
                        createDate = t.createDate + " " + t.createDate.ToString("hh:mm"),
                        lTryDate = t.lTryDate != null ? (t.lTryDate.ToFaDate() + " " + t.lTryDate.Value.ToString("hh:mm")) : "",
                        t.countTry,
                        isSuccess = t.isSuccess == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        ip = t.ip1 + "." + t.ip2 + "." + t.ip3 + "." + t.ip4,
                        t.lastError
                    })
                    .ToList()
            };
        }

        public void SaveChange()
        {
            db.SaveChanges();
        }
    }
}
