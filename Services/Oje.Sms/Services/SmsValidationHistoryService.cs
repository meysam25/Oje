using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class SmsValidationHistoryService : ISmsValidationHistoryService
    {
        readonly SmsDBContext db = null;
        public SmsValidationHistoryService(SmsDBContext db)
        {
            this.db = db;
        }

        public int Create(IpSections ipSections, string mobile, int? siteSettingId, SmsValidationHistoryType type)
        {
            int result = RandomService.GenerateRandomNumber(5);

            db.Entry(new SmsValidationHistory
            {
                CreateDate = DateTime.Now,
                Ip1 = ipSections.Ip1,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                ConfirmCode = result,
                MobileNumber = mobile.ToLongReturnZiro(),
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                InvalidCount = 0,
                IsUsed = false,
                PreUsed = false,
                Type = type
            }).State = EntityState.Added;

            db.SaveChanges();

            return result;
        }

        public int GetLastSecoundFor(SmsValidationHistoryType type, IpSections ipSections, int? siteSettingId)
        {
            int result = int.MaxValue;

            var lastCreateDate =
                db.SmsValidationHistories
                .OrderByDescending(t => t.CreateDate)
                .Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4 && t.Type == type && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();

            if (lastCreateDate != null)
                result = Math.Round((DateTime.Now - lastCreateDate.CreateDate).TotalSeconds).ToIntReturnZiro();

            return result;
        }

        public bool IsValidPreUsed(long mobileNumber, string codeId, IpSections ipSections)
        {
            try
            {
                var curDT = DateTime.Now.AddSeconds(-300);

                var foundItem = db.SmsValidationHistories
                    .Where(t => t.CreateDate >= curDT)
                    .OrderByDescending(t => t.CreateDate)
                    .Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4)
                    .Where(t => t.MobileNumber == mobileNumber && t.InvalidCount <= 2 && t.IsUsed == false && t.PreUsed == true)
                    .FirstOrDefault();

                if (foundItem == null)
                    return false;

                var arrIdParts = codeId.Split(',');
                var ipPart = arrIdParts[0];
                var createDate = new DateTime(arrIdParts[1].ToLongReturnZiro());
                var type = arrIdParts[2].ToIntReturnZiro();

                if (((int)foundItem.Type) != type)
                    return false;
                if (createDate != foundItem.CreateDate)
                    return false;
                if (ipPart != (foundItem.Ip1 + "." + foundItem.Ip2 + "." + foundItem.Ip3 + "." + foundItem.Ip4))
                    return false;

                foundItem.IsUsed = true;
                db.SaveChanges();

                return foundItem.IsUsed.Value;

            }
            catch
            {
                return false;
            }

        }

        public bool ValidateBy(long mobileNumber, int smsCode, IpSections ipSections)
        {
            bool result = false;

            var curDT = DateTime.Now.AddSeconds(-300);

            var foundItem = db.SmsValidationHistories
                .Where(t => t.CreateDate >= curDT)
                .OrderByDescending(t => t.CreateDate)
                .Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4)
                .Where(t => t.MobileNumber == mobileNumber && t.InvalidCount <= 2 && t.IsUsed == false)
                .FirstOrDefault();

            if (foundItem != null)
            {
                if (foundItem.ConfirmCode == smsCode)
                {
                    result = true;
                    foundItem.IsUsed = true;
                    db.SaveChanges();
                }
                else
                {
                    if (foundItem.InvalidCount == null)
                        foundItem.InvalidCount = 0;
                    foundItem.InvalidCount++;
                    db.SaveChanges();
                }
            }

            return result;
        }

        public string ValidatePreUsedBy(long mobileNumber, int smsCode, IpSections ipSections)
        {
            string result = null;

            var curDT = DateTime.Now.AddSeconds(-300);

            var foundItem = db.SmsValidationHistories
                .Where(t => t.CreateDate >= curDT)
                .OrderByDescending(t => t.CreateDate)
                .Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4)
                .Where(t => t.MobileNumber == mobileNumber && t.InvalidCount <= 2 && t.IsUsed == false)
                .FirstOrDefault();

            if (foundItem != null)
            {
                if (foundItem.ConfirmCode == smsCode)
                {
                    result = foundItem.Ip1 + "." + foundItem.Ip2 + "." + foundItem.Ip3 + "." + foundItem.Ip4 + "," + foundItem.CreateDate.Ticks + "," + ((int)foundItem.Type);
                    result = result.Encrypt2();
                    foundItem.PreUsed = true;
                    db.SaveChanges();
                }
                else
                {
                    if (foundItem.InvalidCount == null)
                        foundItem.InvalidCount = 0;
                    foundItem.InvalidCount++;
                    db.SaveChanges();
                }
            }

            return result;
        }
    }
}
