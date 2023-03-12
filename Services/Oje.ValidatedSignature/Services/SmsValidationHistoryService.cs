using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class SmsValidationHistoryService : ISmsValidationHistoryService
    {
        readonly ValidatedSignatureDBContext db = null;
        public SmsValidationHistoryService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(string id)
        {
            string result = "";

            IpSections ipSections = null;
            DateTime? createDate = null;
            SmsValidationHistoryType? type = null;

            try
            {
                var allParts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
                if (allParts.Length == 6)
                {
                    ipSections = new IpSections() { Ip1 = allParts[0].ToByteReturnZiro(), Ip2 = allParts[1].ToByteReturnZiro(), Ip3 = allParts[2].ToByteReturnZiro(), Ip4 = allParts[3].ToByteReturnZiro() };
                    createDate = new DateTime(allParts[4].ToLongReturnZiro());
                    type = (SmsValidationHistoryType)allParts[5].ToIntReturnZiro();
                }
            }
            catch { }

            if (ipSections == null || createDate == null || type == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundItem = db.SmsValidationHistories
                .FirstOrDefault(t => 
                    t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4 
                    && t.CreateDate == createDate && t.Type == type
                );

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in SmsValidationHistories" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<SmsValidationHistoryMainGridResultVM> GetList(SmsValidationHistoryMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SmsValidationHistories
                .Include(t => t.SiteSetting)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var ipParts = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == ipParts.Ip1 && t.Ip2 == ipParts.Ip2 && t.Ip3 == ipParts.Ip3 && t.Ip4 == ipParts.Ip4);
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
            if (searchInput.isUsed != null)
                quiryResult = quiryResult.Where(t => t.IsUsed == searchInput.isUsed);
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));


            bool hasSort = false;

            if (searchInput.sortField == "ip" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Ip1).ThenBy(t => t.Ip2).ThenBy(t => t.Ip3).ThenBy(t => t.Ip4);
            }
            else if (searchInput.sortField == "ip" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Ip1).ThenByDescending(t => t.Ip2).ThenByDescending(t => t.Ip3).ThenByDescending(t => t.Ip4);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.CreateDate);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
            }
            else if (searchInput.sortField == "type" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Type);
            }
            else if (searchInput.sortField == "type" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Type);
            }
            else if (searchInput.sortField == "mobile" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.MobileNumber);
            }
            else if (searchInput.sortField == "mobile" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.MobileNumber);
            }
            else if (searchInput.sortField == "isUsed" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.IsUsed);
            }
            else if (searchInput.sortField == "isUsed" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.IsUsed);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);


            int row = searchInput.skip;

            return new GridResultVM<SmsValidationHistoryMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new SmsValidationHistoryMainGridResultVM
                {
                    createDate = t.CreateDate.ToFaDate(),
                    id = t.Ip1 + "_" + t.Ip2 + "_" + t.Ip3 + "_" + t.Ip4 + "_" + t.CreateDate.Ticks + "_" + ((int)t.Type),
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    isUsed = t.IsUsed == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    mobile = t.MobileNumber,
                    row = ++row,
                    type = t.Type.GetEnumDisplayName(),
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    isValid = t.IsSignature() ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
