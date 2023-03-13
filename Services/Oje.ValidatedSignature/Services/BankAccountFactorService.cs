using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.DB;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;
using System.Security.Cryptography;

namespace Oje.ValidatedSignature.Services
{
    public class BankAccountFactorService : IBankAccountFactorService
    {
        readonly ValidatedSignatureDBContext db = null;
        public BankAccountFactorService(ValidatedSignatureDBContext db)
        {
            this.db = db;
        }

        public object GetBy(string id)
        {
            string result = "";

            int? bankAccountId = null;
            BankAccountFactorType? type = null;
            long? objectId = null;
            DateTime? createDate = null;

            try
            {
                var allIdParts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
                if (allIdParts.Length == 4)
                {
                    bankAccountId = allIdParts[0].ToIntReturnZiro();
                    type = (BankAccountFactorType)allIdParts[1].ToIntReturnZiro();
                    objectId = allIdParts[2].ToLongReturnZiro();
                    createDate = new DateTime(allIdParts[3].ToLongReturnZiro());
                }
            }
            catch { }

            var foundItem = db.BankAccountFactors
                .FirstOrDefault(t => t.BankAccountId == bankAccountId && t.Type == type && t.ObjectId == objectId && t.CreateDate == createDate);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in " + nameof(BankAccountFactor) + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<BankAccountFactorMainGridResultVM> GetList(BankAccountFactorMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.BankAccountFactors
                .Include(t => t.SiteSetting)
                .Include(t => t.User)
                .Include(t => t.BankAccount)
                .AsQueryable();


            if (!string.IsNullOrEmpty(searchInput.bcTitle))
                quiryResult = quiryResult.Where(t => t.BankAccount.Title.Contains(searchInput.bcTitle));
            if (searchInput.objId.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.ObjectId == searchInput.objId);
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.user))
                quiryResult = quiryResult.Where(t => t.User.Username.Contains(searchInput.user) || (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.user));
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (searchInput.isPayed != null)
                quiryResult = quiryResult.Where(t => t.IsPayed == searchInput.isPayed);
            if (!string.IsNullOrEmpty(searchInput.traceCode))
                quiryResult = quiryResult.Where(t => !string.IsNullOrEmpty(t.TraceCode) && t.TraceCode.Contains(searchInput.traceCode));
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            bool hasSort = false;
            if (!string.IsNullOrEmpty(searchInput.sortField))
            {
                if (searchInput.sortField == "bcTitle" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.BankAccountId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "bcTitle" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.BankAccountId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "objId" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.ObjectId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "objId" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.ObjectId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "type" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Type);
                    hasSort = true;
                }
                else if (searchInput.sortField == "type" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Type);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    hasSort = true;
                }
                else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.Price);
                    hasSort = true;
                }
                else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.Price);
                    hasSort = true;
                }
                else if (searchInput.sortField == "isPayed" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.IsPayed);
                    hasSort = true;
                }
                else if (searchInput.sortField == "isPayed" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.IsPayed);
                    hasSort = true;
                }
                else if (searchInput.sortField == "traceCode" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.TraceCode);
                    hasSort = true;
                }
                else if (searchInput.sortField == "traceCode" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.TraceCode);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
                {
                    quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
                    hasSort = true;
                }
                else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
                {
                    quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
                    hasSort = true;
                }
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);

            int row = searchInput.skip;


            return new GridResultVM<BankAccountFactorMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new BankAccountFactorMainGridResultVM()
                {
                    row = ++row,
                    id = t.BankAccountId + "_" + ((int) t.Type) + "_" + t.ObjectId + "_" + t.CreateDate.Ticks,
                    bcTitle = t.BankAccount != null ? t.BankAccount.Title : "",
                    objId = t.ObjectId + "",
                    type = t.Type.GetEnumDisplayName(),
                    createDate = t.CreateDate.ToFaDate(),
                    price = t.Price.ToString("###,###"),
                    isPayed = t.IsPayed ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    traceCode = t.TraceCode,
                    website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                    user = t.User != null 
                    ? 
                        t.User.Username  + (!string.IsNullOrEmpty(t.User.Firstname) ? ("(" + t.User.Firstname + " " + t.User.Lastname + ")") : "")
                    : 
                    "",
                    isValid = t.IsSignature() ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
