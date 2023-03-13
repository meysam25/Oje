using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sms.Services
{
    public class SmsValidationHistoryService : ISmsValidationHistoryService
    {
        readonly SmsDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public SmsValidationHistoryService(SmsDBContext db, IHttpContextAccessor HttpContextAccessor)
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public int Create(IpSections ipSections, string mobile, int? siteSettingId, SmsValidationHistoryType type)
        {
            int result = RandomService.GenerateRandomNumber(5);

            var newItem = new SmsValidationHistory
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
            };
            newItem.FilledSignature();
            db.Entry(newItem).State = EntityState.Added;

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

        public GridResultVM<SmsValidationHistoryMainGridResultVM> GetList(SmsValidationHistoryMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new SmsValidationHistoryMainGrid();

            var quiryResult = db.SmsValidationHistories.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var targetIp = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == targetIp.Ip1 && t.Ip2 == targetIp.Ip2 && t.Ip3 == targetIp.Ip3 && t.Ip4 == targetIp.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (searchInput.mobile != null)
                quiryResult = quiryResult.Where(t => t.MobileNumber == searchInput.mobile);
            if (searchInput.invalidCount != null && searchInput.invalidCount.Value >= 0)
                quiryResult = quiryResult.Where(t => t.InvalidCount == searchInput.invalidCount.Value);
            if (searchInput.isUsed != null)
                quiryResult = quiryResult.Where(t => t.IsUsed == searchInput.isUsed);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            switch (searchInput.sortField)
            {
                case "ip":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Ip1).ThenByDescending(t => t.Ip2).ThenByDescending(t => t.Ip3).ThenByDescending(t => t.Ip4);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Ip1).ThenBy(t => t.Ip2).ThenBy(t => t.Ip3).ThenBy(t => t.Ip4);
                    break;
                case "createDate":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    break;
                case "mobile":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.MobileNumber);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.MobileNumber);
                    break;
                case "invalidCount":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.InvalidCount);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.InvalidCount);
                    break;
                case "isUsed":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.IsUsed);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.IsUsed);
                    break;
                case "type":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Type);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Type);
                    break;
                default:
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    break;
            }

            int row = searchInput.skip;

            return new GridResultVM<SmsValidationHistoryMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.CreateDate,
                    t.Type,
                    t.MobileNumber,
                    t.InvalidCount,
                    t.IsUsed,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new SmsValidationHistoryMainGridResultVM
                {
                    row = ++row,
                    id = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("HH:mm"),
                    type = t.Type.GetEnumDisplayName(),
                    mobile = t.MobileNumber.ToString(),
                    invalidCount = t.InvalidCount + "",
                    isUsed = t.IsUsed == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
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
                if (!foundItem.IsSignature())
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
                foundItem.FilledSignature();
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
                if (!foundItem.IsSignature())
                    return result;
                if (foundItem.ConfirmCode == smsCode)
                {
                    result = true;
                    foundItem.IsUsed = true;
                    foundItem.FilledSignature();
                    db.SaveChanges();
                }
                else
                {
                    if (foundItem.InvalidCount == null)
                        foundItem.InvalidCount = 0;
                    foundItem.InvalidCount++;
                    foundItem.FilledSignature();
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
                if (!foundItem.IsSignature())
                    return result;
                if (foundItem.ConfirmCode == smsCode)
                {
                    result = foundItem.Ip1 + "." + foundItem.Ip2 + "." + foundItem.Ip3 + "." + foundItem.Ip4 + "," + foundItem.CreateDate.Ticks + "," + ((int)foundItem.Type);
                    result = result.Encrypt2();
                    foundItem.PreUsed = true;
                    foundItem.FilledSignature();
                    db.SaveChanges();
                }
                else
                {
                    if (foundItem.InvalidCount == null)
                        foundItem.InvalidCount = 0;
                    foundItem.InvalidCount++;
                    foundItem.FilledSignature();
                    db.SaveChanges();
                }
            }

            return result;
        }
    }
}
